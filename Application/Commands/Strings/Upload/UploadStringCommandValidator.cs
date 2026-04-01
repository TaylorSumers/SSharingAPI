using FluentValidation;

namespace Application.Commands.Strings.Upload
{
    public class UploadStringCommandValidator : AbstractValidator<UploadStringCommand>
    {
        public UploadStringCommandValidator()
        {
            RuleFor(cmd => cmd.Value).NotEmpty();
        }
    }
}
