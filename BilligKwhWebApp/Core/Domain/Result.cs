using System;

namespace BilligKwhWebApp.Core.Domain
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Message { get; }

        protected Result(bool isSuccess, string message)
        {
            if (isSuccess && !string.IsNullOrEmpty(message))
                throw new InvalidOperationException();
            if (!isSuccess && string.IsNullOrEmpty(message))
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default(T), false, message);
        }

        public static Result Ok()
        {
            return new Result(true, string.Empty);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, string.Empty);
        }

        public static Result Combine(params Result[] results)
        {
            foreach (Result result in results)
            {
                if (result.IsFailure)
                    return result;

            }
            return Ok();
        }
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public T Value
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _value;
            }
        }
        protected internal Result(T value, bool isSuccess, string message)
            : base(isSuccess, message)
        {
            _value = value;
        }
    }
}
