using FluentValidation.Results;

namespace ZgnWebApi.Core.Utilities.Middlewares
{
    public class ValidationErrorDetails : ErrorDetails
    {
        public IEnumerable<ValidationFailure>? Errors { get; set; }
    }

}
