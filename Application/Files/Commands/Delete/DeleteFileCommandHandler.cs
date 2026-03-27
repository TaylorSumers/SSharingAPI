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
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(note => note.Code == request.Code, cancellationToken);

            // delete from cloud
            S3ObjectDeleteResponse response = await _storageService.ObjectService.DeleteAsync(dbFile.Name);

            _dbContext.Files.Remove(dbFile);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
