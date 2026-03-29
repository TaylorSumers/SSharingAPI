using MediatR;

namespace Application.Files.Commands.Upload
{
    public class UploadStringCommand : IRequest<string>
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public byte[] FileContent { get; set; }
        public bool DeleteAfterDownload { get; set; }
    }
}
