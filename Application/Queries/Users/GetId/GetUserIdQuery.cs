using MediatR;

namespace Application.Queries.Users.GetId
{
    public class GetUserIdQuery : IRequest<int>
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
