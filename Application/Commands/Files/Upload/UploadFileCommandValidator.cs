using FluentValidation;

namespace Application.Commands.Files.Upload
{
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(cmd => cmd.FileContent).NotEmpty();
            RuleFor(cmd => cmd.Name).NotEmpty();
        }
    }
}
