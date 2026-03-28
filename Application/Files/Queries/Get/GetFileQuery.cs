using MediatR;

namespace Application.Files.Queries.Get
{
    public class GetFileQuery : IRequest<FileVm>
    {
        public Guid Code { get; set; }
    }
}
