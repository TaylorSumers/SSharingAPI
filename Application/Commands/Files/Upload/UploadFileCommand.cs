using MediatR;

namespace Application.Commands.Files.Upload
{
    public class UploadFileCommand : IRequest<string>
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] FileContent { get; set; }
        public bool DeleteAfterDownload { get; set; }
    }
}
