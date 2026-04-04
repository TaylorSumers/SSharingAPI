using Application.Commands.Users.Create;
using Application.Queries.Users.GetId;
using Microsoft.AspNetCore.Mvc;

namespace SecretsSharingAPI.Controllers
{
    public class UsersController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetId(GetUserIdQuery getUserIdQuery)
        {
            int userId = await Mediator.Send(getUserIdQuery);
            return Ok(userId);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateUserCommand createUserCommand)
        {
            await Mediator.Send(createUserCommand);
            return NoContent();
        }
    }
}
