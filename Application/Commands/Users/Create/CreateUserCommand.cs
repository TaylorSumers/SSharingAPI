using MediatR;

namespace Application.Commands.Users.Create
{
    public class CreateUserCommand : IRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
