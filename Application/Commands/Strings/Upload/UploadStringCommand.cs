using MediatR;

namespace Application.Commands.Strings.Upload
{
    public class UploadStringCommand : IRequest<string>
    {
        public string Value { get; set; }
        public bool DeleteAfterDownload { get; set; }
        public int UserId { get; set; }
    }
}
