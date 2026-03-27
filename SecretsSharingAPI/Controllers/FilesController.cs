using Application.Files.Commands.Upload;
using Application.Files.Queries.GetFile;
using AspNetCore.Yandex.ObjectStorage;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SecretsSharingAPI.Models;

namespace SecretsSharingAPI.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : BaseController
    {
        private readonly IMapper _mapper;
        public FilesController(IMapper mapper)
        {
            _mapper = mapper;
        }


        [HttpGet("{code}")]
        public async Task<ActionResult<FileVm>> GetFile(Guid code)
        {
            var fileQuery = new GetFileQuery
            {
                Code = code
            };
            var fileVm = await Mediator.Send(fileQuery);
            return Ok(fileVm);
        }

        //[HttpPost]
        //public async Task<ActionResult<Guid>> UploadFile([FromBody] UploadFileDto uploadFileDto)
        //{
        //    var uploadCommand = _mapper.Map<UploadFileCommand>(uploadFileDto);
        //    uploadCommand.UserId = UserId;
        //    var fileUrl = await Mediator.Send(uploadCommand);
        //    return Ok(fileUrl);
        //}
    }
}
