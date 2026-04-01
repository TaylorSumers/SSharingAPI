using FluentValidation;

namespace Application.Commands.Strings.Delete
{
    public class DeleteStringCommandValidator : AbstractValidator<DeleteStringCommand>
    {
        public DeleteStringCommandValidator()
        {
            RuleFor(cmd => cmd.Code).NotEqual(Guid.Empty);
        }
    }
}
