using Application.Commands.Strings.Delete;
using Application.Commands.Strings.Upload;
using Application.Queries.Strings.Get;
using Application.Queries.Strings.GetList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace SecretsSharingAPI.Controllers
{
    public class StringsController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpGet("{code}")]
        public async Task<ActionResult<StringVm>> Get(Guid code)
        {
            var strQuery = new GetStringQuery
            {
                Code = code
            };
            var strVm = await Mediator.Send(strQuery);

            if (strVm.DeleteAfterDownload)
            {
                var delCmd = new DeleteStringCommand
                {
                    Code = strQuery.Code
                };
                await Mediator.Send(delCmd);
            }

            return Ok(strVm);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<StringLookupDto>>> GetList(int userId)
        {
            var strQuery = new GetListQuery
            {
                UserId = userId
            };
            var fileList = await Mediator.Send(strQuery);
            return Ok(fileList);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Upload([FromBody] UploadStringCommand uploadStringCommand)
        {
            var fileUrl = await Mediator.Send(uploadStringCommand);
            return Ok(fileUrl);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid code)
        {
            var command = new DeleteStringCommand
            {
                Code = code
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
