using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Files.Delete
{
    public class DeleteStringCommandHandler : IRequestHandler<DeleteFileCommand>
    {
        private readonly ISecretsDbContext _dbContext;
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public DeleteStringCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService)
        {
            _dbContext = dbContext;
            _storageService = storageService;
        }

        public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken) // TODO: Обработка исключений
        {
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == file.Code, cancellationToken);
            if (dbFile is null) 
            {
                throw new NotFoundException(nameof(Domain.File), request.Code);
            }

            // delete from cloud
            var fileName = $"{dbFile.Code}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            var response = await _storageService.ObjectService.DeleteAsync(fileName);
            if (!response.IsSuccessStatusCode)
            {
                var content = await response.ReadResultAsStringAsync();
                throw new S3RequestException(content.Errors);
            }

            // delete from db
            _dbContext.Files.Remove(dbFile);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
