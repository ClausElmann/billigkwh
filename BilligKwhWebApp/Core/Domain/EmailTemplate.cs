namespace BilligKwhWebApp.Core.Domain
{
    public class EmailTemplate : BaseEntity
    {
        // Props
        public string Name { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Html { get; set; }
        public string ReplyTo { get; set; }
        public string BccEmails { get; set; }
        // Keys??
        public int LanguageId { get; set; }
        public bool? UseHtmlFromDb { get; set; }

        // Ctors
        public EmailTemplate() { }
        public EmailTemplate(EmailTemplate createFrom)
        {
            Id = createFrom.Id;
            Name = createFrom.Name;
            LanguageId = createFrom.LanguageId;
            Subject = createFrom.Subject;
            Html = createFrom.Html;
            FromEmail = createFrom.FromEmail;
            FromName = createFrom.FromName;
            ReplyTo = createFrom.ReplyTo;
            BccEmails = createFrom.BccEmails;
        }

        public EmailTemplate MergeMasterMainContent(string text)
        {
            Html = Html.Replace("$$MAIN_CONTENT$$", text);
            return this;
        }

        /// <summary>
        /// These are Refactored For SupportCase only.
        /// Have to check before using on other templates..
        /// </summary>
        public EmailTemplate Merge_Text_HtmlField(string text)
        {
            Html = Html.Replace("$$TEXT$$", text);

            return this;
        }
       
        ///----------------------------------------------------
    }
}
