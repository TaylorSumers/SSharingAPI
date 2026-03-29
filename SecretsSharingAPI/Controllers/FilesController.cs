using Application.Files.Commands.Delete;
using Application.Files.Commands.Upload;
using Application.Files.Queries.Get;
using Application.Files.Queries.GetList;
using Application.Queries.Strings.Get;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretsSharingAPI.Models;

namespace SecretsSharingAPI.Controllers
{
    public class FilesController : BaseController
    {
        [HttpGet("{code}")]
        public async Task<ActionResult<FileVm>> Get(Guid code)
        {
            var fileQuery = new GetFileQuery
            {
                Code = code
            };
            var fileVm = await Mediator.Send(fileQuery);
            return Ok(fileVm);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<List<FileLookupDto>>> GetList(int userId)
        {
            var fileQuery = new GetListQuery
            {
                UserId = userId
            };
            var fileList = await Mediator.Send(fileQuery);
            return Ok(fileList);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Upload([FromBody] UploadFileDto uploadFileDto)
        {
            var uploadFileCommand = Mapper.Map<UploadStringCommand>(uploadFileDto);
            var fileUrl = await Mediator.Send(uploadFileCommand);
            return Ok(fileUrl);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid code) 
        {
            var command = new DeleteFileCommand
            {
                Code = code
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
