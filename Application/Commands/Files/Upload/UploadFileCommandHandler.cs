using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using MediatR;
using DbFile = Domain.File;

namespace Application.Commands.Files.Upload
{
    public class UploadStringCommandHandler : IRequestHandler<UploadStringCommand, string>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public UploadStringCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task<string> Handle(UploadStringCommand request, CancellationToken cancellationToken)
        {
            //Send to cloud
            Guid fileCode = Guid.NewGuid();
            var response = await _storageService.ObjectService.PutAsync(request.FileContent, $"{fileCode}{request.Name.Substring(request.Name.LastIndexOf('.'))}"); // TODO: получать название для хранения в S3 отдельном методе
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

            return $"https://localhost:44306/api/Files/GetFile/code={dbFile.Code}"; // TODO: определять url хоста
        }
    }
}
