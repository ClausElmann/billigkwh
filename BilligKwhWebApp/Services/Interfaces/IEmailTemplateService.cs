using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;
using System.Collections.Generic;

namespace BilligKwhWebApp.Services.Interfaces
{

    public interface IEmailTemplateService
    {
        EmailTemplate GetMasterTemplate(int languageId);

        EmailTemplate GetTemplateByNameEnum(int languageId,
                                            EmailTemplateName templateName,
                                            EmailTemplateName masterTempateEnum = EmailTemplateName.MasterTemplate);

        string EmailFromFormatted(EmailTemplate template);

        void MergeEmailFields(EmailTemplate template, List<KeyValuePair<string, object>> fields);
        void RemoveEmailTags(EmailTemplate template, List<string> tags);

        List<EmailTemplate> GetAllTemplates();

        void Insert(EmailTemplate emailTemplate);
        void Update(EmailTemplate emailTemplate);
        void Delete(EmailTemplate emailTemplate);
        string GetTemplateHtmlByNameEnum(int languageId, EmailTemplateName templateName);
    }
}
