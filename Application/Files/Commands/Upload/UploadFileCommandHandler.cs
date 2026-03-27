using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using MediatR;
using DbFile = Domain.File;

namespace Application.Files.Commands.Upload
{
    public class UploadFileCommandHandler : IRequestHandler<UploadFileCommand, string>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public UploadFileCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<string> Handle(UploadFileCommand request, CancellationToken cancellationToken)
        {
            //Send to cloud
            Guid fileCode = Guid.NewGuid();
            S3ObjectPutResponse response = await _storageService.ObjectService.PutAsync(request.FileContent, $"{fileCode}{request.Name.Substring(request.Name.LastIndexOf('.'))}"); // TODO: получать название для хранения в S3 проще

            var dbFile = new DbFile
            {
                Name = request.Name,
                Code = fileCode,
                DeleteAfterDownload = request.DeleteAfterDownload
            };

            await _dbContext.Files.AddAsync(dbFile, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return $"https://localhost:7146/api/Files/GetFile/code={dbFile.Code}"; // TODO: определять url хоста
        }
    }
}
