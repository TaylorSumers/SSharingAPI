using MediatR;

namespace Application.Commands.Strings.Delete
{
    public class DeleteStringCommand : IRequest
    {
        public Guid Code;
    }
}
