using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using AspNetCore.Yandex.ObjectStorage.Object.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Files.Commands.Delete
{
    public class DeleteFileCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public DeleteFileCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken) // TODO: Обработка исключений
        {
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == file.Code, cancellationToken);

            // delete from cloud
            var fileName = $"{dbFile.Code}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            S3ObjectDeleteResponse response = await _storageService.ObjectService.DeleteAsync(fileName);

            // delete from db
            _dbContext.Files.Remove(dbFile);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
