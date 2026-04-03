using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Buffers.Text;
using DbFile = Domain.File;

namespace Application.Commands.Files.Upload
{
    public class UploadFileCommandHandler : HandlerBase<UploadFileCommand, string>
    {
        private readonly IYandexStorageService _storageService;
        private readonly IConfiguration _configuration;

        public UploadFileCommandHandler(ISecretsDbContext dbContext, IYandexStorageService storageService, IConfiguration configuration) : base(dbContext)
        {
            _storageService = storageService;
            _configuration = configuration;
        }

        public override async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            //Send to cloud
            var fileCode = Guid.NewGuid();
            var cloudFileName = $"{fileCode}{request.Name.Substring(request.Name.LastIndexOf('.'))}";
            var response = await _storageService.ObjectService.PutAsync(request.FileContent, cloudFileName);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.ReadResultAsStringAsync();
                throw new S3RequestException(content.Errors);
            }

            //Add to db
            var dbFile = new DbFile
            {
                Name = request.Name,
                Code = fileCode,
                DeleteAfterDownload = request.DeleteAfterDownload
            };
            await _dbContext.Files.AddAsync(dbFile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return $"{_configuration["PublicApiBaseUrl"]}/api/Files/Get/{fileCode}";
        }
    }
}
