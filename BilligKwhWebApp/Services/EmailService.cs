using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Core.Toolbox;
using BilligKwhWebApp.Services.Enums;
using BilligKwhWebApp.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System;
using Z.Dapper.Plus;
using System.Linq;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Models;
using BilligKwhWebApp.Core;
using System.Net;

namespace BilligKwhWebApp.Services
{
    public class EmailService : IEmailService
    {
        // Props
        private readonly IBaseRepository _baseRepository;
        private readonly ISystemLogger _systemLogger;

        // Ctor
        public EmailService(IBaseRepository baseRepository,
                            IEmailTemplateService emailTemplateService,
                            ISystemLogger systemLogger)
        {
            _baseRepository = baseRepository;
            _systemLogger = systemLogger;
        }

        public bool Save(
            int customerId, string fromEmail, string fromName, string sendTo, string sendToName, string replyTo, string subject, string body,
            EmailCategoryEnum categoryEnum, int? refTypeID = null, int? refID = null, string bccEmails = null, IEnumerable<Tuple<byte[],
                string>> attachments = null, Cc_Bcc isCc_or_Bcc = Cc_Bcc.BCC
           )
        {
            var mailMessage = new EmailMessage
            {
                DateCreatedUtc = DateTime.UtcNow,

                CustomerId = customerId,
                FromEmail = fromEmail,
                FromName = fromName,
                ToEmail = sendTo,
                ToName = sendToName,

                Body = body,

                CategoryId = (int)categoryEnum,
                RefTypeID = refTypeID,
                RefID = refID,

                Subject = subject.Truncate(200),

                BccEmails = bccEmails,
                ReplyTo = replyTo.Truncate(200),

                HasAttachments = false,
                UseBcc = isCc_or_Bcc == Cc_Bcc.BCC,
            };

            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                connection.BulkInsert(mailMessage);

                if (attachments != null)
                {
                    List<EmailAttachment> emailAttachments = new List<EmailAttachment>();
                    foreach (var attachment in attachments)
                    {
                        if (attachment.Item1.Length > 0)
                        {
                            emailAttachments.Add(new EmailAttachment
                            {
                                FileContent = attachment.Item1,
                                FileName = attachment.Item2,
                                MessageId = mailMessage.Id,
                            });
                            mailMessage.HasAttachments = true;
                        }
                    }
                    connection.BulkInsert(emailAttachments);
                }
                connection.BulkUpdate(mailMessage);
            }
#if DEBUG
            //SendMail(mailMessage.Id);
            if (Environment.MachineName == "LAPTOP-2Q5GS5I8")
                SendTestMailFromDev(mailMessage.Id);
            else
                SendMail(mailMessage.Id);
#else
            SendMail(mailMessage.Id);
#endif

            return true;
        }

        public Result Save(EmailMessage emailMessage)
        {
            // ToDo: Create Atomic Commit Instead
            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                // Insert
                connection.BulkInsert(emailMessage);

                emailMessage.Attachments.ForEach(attachment => attachment.SetMessageId(emailMessage.Id));

                connection.BulkInsert(emailMessage.Attachments);
            }
            return Result.Ok();
        }
        public void Save(IEnumerable<EmailMessage> emailMessages)
        {
            _baseRepository.BulkInsert(emailMessages);
        }

        // CRUD
        public Result<EmailMessage> Get(int id)
        {
            var param = new { Id = id };
            var sql = @"
                SELECT *                
                FROM dbo.EmailMessages
                WHERE Id = @Id";

            var result = _baseRepository.Query<EmailMessage>(sql, param).FirstOrDefault();
            if (result == null)
            {
                return Result.Fail<EmailMessage>("Email with " + id + " could not be found!");
            }
            return Result.Ok(result);
        }
        public Result Update(EmailMessage emailMessage)
        {
            var result = _baseRepository.Update(emailMessage);
            if (result == false)
            {
                return Result.Fail("EmailMessage with Id: " + emailMessage.Id + "could not be updated.");
            }
            return Result.Ok();
        }

        /// <summary>
        /// To be used for testing purposes inn DEV, from batchapp
        /// </summary>
        /// <param name="messageId">Id of EmailMessage</param>
        /// <param name="mailAccount">Office 365 mail acount username</param>
        /// <param name="password">Office 365 mail acount password</param>
        public void SendTestMailFromDev(int messageId)
        {
            string mailAccount = "noreply@billigkwh.dk";
            string password = "k3dJKUl6Ej$7";

            var getMessagesSql = @"
                    SELECT *
					FROM dbo.EmailMessages
					WHERE Id = @MessageId";
            var getParam = new { MessageId = messageId };

            var emailsToSend = _baseRepository.Query<EmailMessage>(getMessagesSql, getParam);

            if (emailsToSend.Count() != 1) return;
            var mail = emailsToSend.Single();

            MailMessage msg = new(new MailAddress("noreply@billigkwh.dk", "BilligKwh"), new MailAddress("claus.elmann@gmail.com", "Claus Elmann"))
            {
                Subject = mail.Subject,
                Body = mail.Body,
                IsBodyHtml = true
            };

            var getAttachmentSql = "SELECT * FROM dbo.EmailAttachments WHERE MessageId = @MessageId";
            var attachments = _baseRepository.Query<EmailAttachment>(getAttachmentSql, new { MessageId = mail.Id }).ToList();
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    msg.Attachments.Add(new Attachment(new MemoryStream(attachment.FileContent), attachment.FileName));
                }
            }

            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailAccount, password),
                Port = 587,
                Host = "smtp.simply.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            client.Send(msg);
            mail.DateSentUtc = DateTime.UtcNow;
            Update(mail);
        }

        public void SendMail(int messageId)
        {
            var getMessagesSql = @"
                    SELECT *
					FROM dbo.EmailMessages
					WHERE Id = @MessageId";
            var getParam = new { MessageId = messageId };

            var emailsToSend = _baseRepository.Query<EmailMessage>(getMessagesSql, getParam);

            if (emailsToSend.Count() != 1) return;
            var mail = emailsToSend.Single();

            MailMessage msg = new(new MailAddress("noreply@billigkwh.dk", mail.FromName), new MailAddress(mail.ToEmail, mail.ToName))
            {
                Subject = mail.Subject,
                Body = mail.Body,
                IsBodyHtml = true
            };

            var getAttachmentSql = "SELECT * FROM dbo.EmailAttachments WHERE MessageId = @MessageId";
            var attachments = _baseRepository.Query<EmailAttachment>(getAttachmentSql, new { MessageId = mail.Id }).ToList();
            if (attachments != null && attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    msg.Attachments.Add(new Attachment(new MemoryStream(attachment.FileContent), attachment.FileName));
                }
            }

            SmtpClient client = new()
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("noreply@billigkwh.dk", "k3dJKUl6Ej$7"),
                Port = 587,
                Host = "websmtp.simply.com",
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };

            try
            {
                client.Send(msg);
                mail.DateSentUtc = DateTime.UtcNow;
                Update(mail);
            }
            catch (Exception ex)
            {
                _systemLogger.Fatal($"Failed to send E-mail {mail.ToEmail} / {mail.Subject} / {ex.Message}", ex);
                throw;
            }
        }

        private IEnumerable<EmailMessage> GetQueuedEmails(int count)
        {
            // Get Queued Emails
            var getSql = @"
                    SELECT TOP (@Count) *
					FROM dbo.EmailMessages
					WHERE Status = @Status
                    ORDER BY PriorityOrder";
            var getParam = new { Count = count, Status = EmailStatus.Queued };

            var emailsToSend = _baseRepository.Query<EmailMessage>(getSql, getParam);

            // Update Email Status
            var updateSql = @"
                UPDATE dbo.EmailMessages 
                    SET Status = @Status,
                    DateChangedUtc = GETUTCDATE()  
                WHERE Id IN @emailIds 
                    AND Status = @QueuedStatus ";
            var emailIds = emailsToSend.Select(s => s.Id).ToList();
            var updateParam = new { Status = EmailStatus.SendingToGateway, QueuedStatus = EmailStatus.Queued, emailIds };

            _baseRepository.Execute(updateSql, updateParam);

            return emailsToSend;
        }

        public IReadOnlyCollection<EmailModel> GetAll(int? customerId, DateTime fromDate, DateTime toDate)
        {
            #region
            var param = new { CustomerId = customerId, FromDateUtc = fromDate, ToDateUtc = toDate };

            var sql = @"SELECT EmailMessages.*, Kunde.Kundenavn as CustomerName
                      FROM EmailMessages INNER JOIN Kunde ON EmailMessages.CustomerId = Kunde.Id 
                      WHERE (@CustomerId IS NULL OR CustomerId = @CustomerId) AND DateCreatedUtc between @FromDateUtc and dateadd(dy,1, @ToDateUtc)";
            #endregion

            return _baseRepository.Query<EmailModel>(sql, param).ToList();
        }

        public IReadOnlyCollection<int> GetAllUnsent(DateTime fromDate)
        {
            #region
            var param = new { FromDateUtc = fromDate };

            var sql = @"SELECT EmailMessages.Id FROM EmailMessages
                WHERE DateSentUtc IS NULL AND DateCreatedUtc > @FromDateUtc";
            #endregion

            return _baseRepository.Query<int>(sql, param).ToList();
        }

        public IReadOnlyCollection<EmailModel> GetTavleEmails(int tavleId)
        {
            #region
            var param = new { RefID = tavleId, RefTypeID = RefType.ElTavle };

            var sql = @"SELECT EmailMessages.*, Kunde.Kundenavn as CustomerName
                      FROM EmailMessages INNER JOIN Kunde ON EmailMessages.CustomerId = Kunde.Id 
                      WHERE (RefTypeID = @RefTypeID) AND RefID = @RefID";
            #endregion

            return _baseRepository.Query<EmailModel>(sql, param).ToList();
        }

        public IReadOnlyCollection<ElectricityPriceModel> GetAllElectricityPrices(DateTime fromDate, DateTime toDate)
        {
            var param = new { FromDateUtc = fromDate, ToDateUtc = toDate };

            var sql = @"SELECT * 
                      FROM [ElectricityPrices]
                      WHERE [HourUTC] between @FromDateUtc and dateadd(dy,1, @ToDateUtc)";

            return _baseRepository.Query<ElectricityPriceModel>(sql, param).ToList();
        }
    }
}
