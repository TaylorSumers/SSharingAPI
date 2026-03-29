using MediatR;

namespace Application.Queries.Files.Get
{
    public class GetFileQuery : IRequest<FileVm>
    {
        public Guid Code { get; set; }
    }
}
