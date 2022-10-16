using System;
using System.Net.Mail;

namespace BilligKwhWebApp.Core.Domain.ValueObjects
{
    public class EmailAddress : ValueObject<EmailAddress>
    {
        public string Email { get; }

        // Regex Email Validator
        //private static readonly Regex EmailValidateRegex = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$");

        // Ctor
        private EmailAddress()
        { }
        private EmailAddress(string value)
        {
            Email = value;
        }

        // Factory
        public static Result<EmailAddress> Create(string email)
        {
            // Trim string..
            email = (email ?? string.Empty).ToLower().Trim();

            // Validation..
            if (email.Length == 0)
            {
                return Result.Fail<EmailAddress>("Email can not be empty");
            }

            try
            {
                return Result.Ok(new EmailAddress(new MailAddress(email).Address));
            }
            catch (Exception)
            {
                return Result.Fail<EmailAddress>("Email Is Not a valid Address");
            }

            #region RegEx Implementation
            // Use this if RegEx is Preffered instead of Mail Class..
            //if (!EmailValidateRegex.IsMatch(email))
            //{
            //    return Result.Fail<EmailAddress>("Email Is Not a valid Address");
            //}

            //return Result.Ok(new EmailAddress(email));
            #endregion
        }

        protected override bool EqualsCore(EmailAddress other)
        {
            return Email.Equals(other.Email, StringComparison.InvariantCultureIgnoreCase);
        }
        protected override int GetHashCodeCore()
        {
            return Email.GetHashCode();
        }
    }
}
