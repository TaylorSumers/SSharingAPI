using FluentValidation;

namespace Application.Files.Commands.Delete
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(deleteFileCommand => deleteFileCommand.Code).NotEqual(Guid.Empty);
        }
    }
}
