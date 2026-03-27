using MediatR;

namespace Application.Files.Commands.Delete
{
    public class DeleteFileCommand : IRequest
    {
        public Guid Code;
    }
}
