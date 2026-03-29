using MediatR;

namespace Application.Queries.Strings.GetList
{
    public class GetListQuery : IRequest<List<StringLookupDto>>
    {
        public int UserId { get; set; }
    }
}
