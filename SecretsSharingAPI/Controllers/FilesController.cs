using Application.Commands.Files.Delete;
using Application.Commands.Files.Upload;
using Application.Queries.Files.Get;
using Application.Queries.Files.GetList;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SecretsSharingAPI.Models;

namespace SecretsSharingAPI.Controllers
{
    public class FilesController(IMediator mediator, IMapper mapper) : BaseController(mediator, mapper)
    {
        [HttpGet("{code}")]
        public async Task<ActionResult<FileVm>> Get(Guid code)
        {
            var fileQuery = new GetFileQuery
            {
                Code = code
            };
            var fileVm = await Mediator.Send(fileQuery);

            if (fileVm.DeleteAfterDownload)
            {
                var delCmd = new DeleteFileCommand
                {
                    Code = fileQuery.Code,
                };
                await Mediator.Send(delCmd);
            }

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
            var uploadFileCommand = Mapper.Map<UploadFileCommand>(uploadFileDto);
            uploadFileCommand.FileContent = Convert.FromBase64String(uploadFileDto.FileContent); // TODO: Вынести из конроллера
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
