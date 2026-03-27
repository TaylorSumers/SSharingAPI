

using FluentValidation;

namespace Application.Files.Commands.Upload
{
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(uploadFileCommand => uploadFileCommand.FileContent).NotEmpty();
            RuleFor(uploadFileCommand => uploadFileCommand.Name).NotEmpty();
        }
    }
}
