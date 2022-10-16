namespace BilligKwhWebApp.Infrastructure.DataTransferObjects.Common
{
    public class ErrorDto
    {
        public ErrorDto(object errorMessage, int logId)
        {
            ErrorMessage = errorMessage;
            LogId = logId;
        }

        /// <summary>
        /// An object describing the error. Either a string or an object with a string message and additional data.
        /// </summary>
        /// <example>Error message</example>
        public object ErrorMessage { get; }
        public int LogId { get; }
    }
}
