using MediatR;

namespace Application.Commands.Files.Delete
{
    public class DeleteFileCommand : IRequest
    {
        public Guid Code;
    }
}
