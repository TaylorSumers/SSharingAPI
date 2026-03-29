using MediatR;

namespace Application.Queries.Strings.Get
{
    public class GetStringQuery : IRequest<StringVm>
    {
        public Guid Code { get; set; }
    }
}
