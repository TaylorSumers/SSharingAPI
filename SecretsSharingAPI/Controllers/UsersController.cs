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
            var getQuery = new GetUserIdQuery { 
                Login = login,
                Password = password 
            };
            int userId = await Mediator.Send(getQuery);
            return Ok(userId);
        }

        [HttpPost]
        public async Task<ActionResult> Create(string login, string password)
        {
            var createCmd = new CreateUserCommand
            {
                Login = login,
                Password = password
            };
            try
            {
                await Mediator.Send(createCmd);
            }
            catch (CreateUserException ex)
            {
                throw ex; // TODO: Ошибка должна отлавливаться в CustomExceptionHandlerMiddleware без перебрасывания
            }
            return NoContent();
        }
    }
}
