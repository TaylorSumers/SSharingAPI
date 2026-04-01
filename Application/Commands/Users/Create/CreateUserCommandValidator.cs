using FluentValidation;

namespace Application.Commands.Users.Create
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(cmd => cmd.Login).NotEmpty();
            RuleFor(cmd => cmd.Password).NotEmpty();
        }
    }
}
