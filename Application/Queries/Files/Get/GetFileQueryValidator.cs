using FluentValidation;

namespace Application.Queries.Files.Get
{
    public class GetFileQueryValidator : AbstractValidator<GetFileQuery>
    {
        public GetFileQueryValidator()
        {
            RuleFor(query => query.Code).NotEqual(Guid.Empty);
        }
    }
}
