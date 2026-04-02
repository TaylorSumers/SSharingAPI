using FluentResults;

namespace Application.Common.Exceptions
{
    public class S3RequestException : Exception
    {
        public S3RequestException(List<IError> errors) 
            : base($"The request to storage sevice was completed with errors: {string.Join(", ", errors.Select(error => error.Message))}") { }
    }
}
