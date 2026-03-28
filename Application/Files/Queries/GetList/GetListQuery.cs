using MediatR;

namespace Application.Files.Queries.GetList
{
    public class GetListQuery : IRequest<List<FileLookupDto>>
    {
        public int UserId { get; set; }
    }
}
