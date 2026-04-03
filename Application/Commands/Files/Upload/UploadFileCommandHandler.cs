using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using MediatR;
using DbFile = Domain.File;

namespace Application.Commands.Files.Upload
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

            return $"https://localhost:44306/api/Files/GetFile/{dbFile.Code}"; // TODO: Убрать хардкод
        }
    }
}
