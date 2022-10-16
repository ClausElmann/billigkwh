using System;
using System.Linq;

namespace BilligKwhWebApp.Core.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject<PhoneNumber>
    {
        public long Number { get; set; }

        // Ctor
        private PhoneNumber(long value)
        {
            Number = value;
        }

        // Public Api
        public static Result<PhoneNumber> Create(long phoneNumber)
        {
            var phoneNumberIsValid = ValidateLength(phoneNumber);
            if (phoneNumberIsValid.IsSuccess)
            {
                return Result.Ok(phoneNumberIsValid.Value);
            }
            return Result.Fail<PhoneNumber>(phoneNumberIsValid.Message);
        }
        public static Result<PhoneNumber> Create(string phoneNumber)
        {
            // Convert from string
            var convertedPhoneNumber = ConvertFromString(phoneNumber);
            if (convertedPhoneNumber.IsSuccess)
            {
                // validate the Converted Value
                var validatedPhoneNumber = ValidateLength(convertedPhoneNumber.Value.Number);
                if (validatedPhoneNumber.IsSuccess)
                {
                    return Result.Ok(validatedPhoneNumber.Value);
                }
                // Fails..
                return Result.Fail<PhoneNumber>(validatedPhoneNumber.Message);
            }
            return Result.Fail<PhoneNumber>(convertedPhoneNumber.Message);
        }

        // Internals
        private static Result<PhoneNumber> ConvertFromString(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Result.Fail<PhoneNumber>("phoneNumber cant be null");
            }
            if (!phoneNumber.All(c => c >= '0' && c <= '9'))
            {
                return Result.Fail<PhoneNumber>("phoneNumber contains non-digit characters");
            }
            return Result.Ok(new PhoneNumber(Convert.ToInt64(phoneNumber)));
        }
        private static Result<PhoneNumber> ValidateLength(long phoneNumber)
        {
            if (phoneNumber <= 0)
            {
                return Result.Fail<PhoneNumber>("PhoneNumber can't be 0.");
            }
            return Result.Ok(new PhoneNumber(phoneNumber));
        }


        protected override bool EqualsCore(PhoneNumber other)
        {
            return Number.ToString().Equals(other.Number.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
        protected override int GetHashCodeCore()
        {
            return Number.GetHashCode();
        }
    }
}
