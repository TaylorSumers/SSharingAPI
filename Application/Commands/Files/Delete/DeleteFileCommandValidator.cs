using FluentValidation;

namespace Application.Commands.Files.Delete
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(deleteFileCommand => deleteFileCommand.Code).NotEqual(Guid.Empty);
        }
    }
}
