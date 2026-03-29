using FluentValidation;

namespace Application.Commands.Files.Upload
{
    public class UploadFileCommandValidator : AbstractValidator<UploadStringCommand>
    {
        public UploadFileCommandValidator()
        {
            RuleFor(uploadFileCommand => uploadFileCommand.FileContent).NotEmpty();
            RuleFor(uploadFileCommand => uploadFileCommand.Name).NotEmpty();
        }
    }
}
