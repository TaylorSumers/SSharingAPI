using FluentValidation;

namespace Application.Queries.Users.GetId
{
    public class GetUserIdQueryValidator : AbstractValidator<GetUserIdQuery>
    {
        public GetUserIdQueryValidator()
        {
            RuleFor(cmd => cmd.Login).NotEmpty();
            RuleFor(cmd => cmd.Password).NotEmpty();
        }
    }
}
