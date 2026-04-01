using FluentValidation;

namespace Application.Queries.Strings.Get
{
    public class GetStringQueryValidator : AbstractValidator<GetStringQuery>
    {
        public GetStringQueryValidator() 
        {
            RuleFor(query => query.Code).NotEqual(Guid.Empty);
        }
    }
}
