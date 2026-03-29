using Application.Commands.Users.Create;
using Application.Queries.Users.GetId;
using Microsoft.AspNetCore.Mvc;

namespace SecretsSharingAPI.Controllers
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetId(string login, string password)
        {
            var getQuery = new GetUserIdQuery { Login = login, Password = password };
            return Ok(await Mediator.Send(getQuery));
        }

        [HttpPost]
        public async Task<ActionResult> Create(string login, string password)
        {
            var createCmd = new CreateUserCommand
            {
                Login = login,
                Password = password
            };
            await Mediator.Send(createCmd);
            return NoContent();
        }
    }
}
