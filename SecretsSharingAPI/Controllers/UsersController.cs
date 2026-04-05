using Application.Commands.Users.Create;
using Application.Queries.Users.GetId;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SecretsSharingAPI.Controllers
{
    public class UsersController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpPost]
        public async Task<ActionResult> Login(GetUserIdQuery getUserIdQuery)
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
