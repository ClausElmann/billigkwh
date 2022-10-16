using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using BilligKwhWebApp.Services.Interfaces;
using BilligKwhWebApp.Core.Interfaces;
using BilligKwhWebApp.Core.Domain;
using BilligKwhWebApp.Services.Enums;

namespace BilligKwhWebApp.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        // Props
        private readonly IBaseRepository _baseRepository;
        private readonly ISystemLogger _logger;

        // Ctor
        public EmailTemplateService(IBaseRepository baseRepository,
            ISystemLogger logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="languageId"></param>
        /// <param name="templateName">Corresponds to EMailTemplateNameEnum. Must match db-field EmailTemplates.Name</param>
        /// <param name="masterTemplateEnum"></param>
        /// <returns></returns>
        public EmailTemplate GetTemplateByNameEnum(int languageId,
                                                    EmailTemplateName templateName,
                                                    EmailTemplateName masterTemplateName = EmailTemplateName.MasterTemplate)
        {
            var parameters = new
            {
                LanguageId = languageId,
                MasterTemplateName = EmailTemplateName.MasterTemplate.ToString(),
                TemplateName = templateName.ToString()
            };

            using (var connection = ConnectionFactory.GetOpenConnection())
            {
                var masterAndTemplate = connection.QueryMultiple(@"
					SELECT TOP(1) * FROM dbo.EmailTemplates 
					WHERE LanguageId = @LanguageId 
					AND Name = @MasterTemplateName;

					SELECT TOP(1) * FROM dbo.EmailTemplates 
					WHERE LanguageId = @LanguageId 
					AND Name = @TemplateName;", parameters);

                var masterTemplate = masterAndTemplate.Read<EmailTemplate>().FirstOrDefault();
                var template = masterAndTemplate.Read<EmailTemplate>().FirstOrDefault();

                if (masterTemplate != null && (masterTemplate.UseHtmlFromDb == null || masterTemplate.UseHtmlFromDb == false))
                {
                    var masterTemplateFromFile = GetTemplateHtmlByNameEnum(languageId, masterTemplateName);
                    if (masterTemplateFromFile != null) masterTemplate.Html = masterTemplateFromFile;
                }

                if (template != null && (template.UseHtmlFromDb == null || template.UseHtmlFromDb == false))
                {
                    var templateFromFile = GetTemplateHtmlByNameEnum(languageId, templateName);
                    if (templateFromFile != null) template.Html = templateFromFile;
                }

                // Now inject template into master-template
                if (template != null && masterTemplateName != EmailTemplateName.None && masterTemplate != null)
                {
                    template.Html = masterTemplate.Html.Replace("$$MAIN_CONTENT$$", template.Html);
                }
                return template;
            }
        }

        public EmailTemplate GetMasterTemplate(int languageId)
        {
            #region SqlQuery
            var param = new
            {
                LanguageId = languageId,
                MasterTemplateName = EmailTemplateName.MasterTemplate.ToString()
            };
            var sql =
                @" SELECT TOP(1) * 
                   FROM dbo.EmailTemplates 
				   WHERE LanguageId = @LanguageId 
				    AND Name = @MasterTemplateName";

            #endregion
            var masterTemplate = _baseRepository.Query<EmailTemplate>(sql, param).FirstOrDefault();

            if (masterTemplate != null && (masterTemplate.UseHtmlFromDb == null || masterTemplate.UseHtmlFromDb == false))
            {
                var masterTemplateFromFile = GetTemplateHtmlByNameEnum(languageId, EmailTemplateName.MasterTemplate);
                if (masterTemplateFromFile != null) masterTemplate.Html = masterTemplateFromFile;
            }

            return masterTemplate;
        }

        public string EmailFromFormatted(EmailTemplate template)
        {
            return BilligKwhWebApp.Core.Toolbox.Tools.EmailFromFormatted(template.FromName, template.FromEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="template"></param>
        /// <param name="fields">List of key/values: where keys are without $$, and values are simple datatypes (string/int/short...)</param>
        public void MergeEmailFields(EmailTemplate template, List<KeyValuePair<string, object>> fields)
        {
            foreach (var field in fields)
            {
                var fieldName = $"$${field.Key.ToUpper()}$$";
                template.Html = template.Html.Replace(fieldName, field.Value == null ? "" : field.Value.ToString());
            }
        }

        public void RemoveEmailTags(EmailTemplate template, List<string> tags)
        {
            foreach (var tag in tags)
            {
                template.Html = Regex.Replace(template.Html, @"<" + tag + ">(.|\n)*?</" + tag + ">", string.Empty, RegexOptions.IgnoreCase);
            }
        }

        public List<EmailTemplate> GetAllTemplates()
        {
            return _baseRepository.Query<EmailTemplate>(@"SELECT * FROM dbo.EmailTemplates").ToList();
        }

        public void Insert(EmailTemplate template)
        {
            _baseRepository.Insert(template);
        }

        public void Update(EmailTemplate template)
        {
            _baseRepository.Update(template);
        }

        public void Delete(EmailTemplate template)
        {
            _baseRepository.Delete(template);
        }

        public string GetTemplateHtmlByNameEnum(int languageId, EmailTemplateName templateName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"BilligKwhWebApp.Resources.EmailTemplates.{templateName}.{languageId}.aspx";

            var r = assembly.GetManifestResourceNames();

            if (assembly.GetManifestResourceNames().Any(w => w == resourceName))
            {
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            return null;
        }

        public static void SaveToFile(EmailTemplate emailTemplate, string folder)
        {
            File.WriteAllText($@"{folder}\{emailTemplate.Name}.{emailTemplate.LanguageId}.aspx", emailTemplate.Html.Replace("  ", " "));
        }

        public void SaveAllToDisc(string folder)
        {
            foreach (var emailTemplate in GetAllTemplates())
            {
                SaveToFile(emailTemplate, folder);
            }
        }
    }
}
