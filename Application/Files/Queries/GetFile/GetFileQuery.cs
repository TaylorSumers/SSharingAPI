using MediatR;

namespace Application.Files.Queries.GetFile
{
    public class GetFileQuery : IRequest<FileVm>
    {
        public Guid Code { get; set; }
    }
}
