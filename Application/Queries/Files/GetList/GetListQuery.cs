using MediatR;

namespace Application.Queries.Files.GetList
{
    public class GetListQuery : IRequest<List<FileLookupDto>>
    {
        public int UserId { get; set; }
    }
}
