using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Files.Delete
{
    public class DeleteFileCommandHandler : HandlerBase<DeleteFileCommand>
    {
        private readonly IYandexStorageService _storageService;

        public DeleteFileCommandHandler(ISecretsDbContext dbContext, IYandexStorageService storageService) : base(dbContext)
        {
            _storageService = storageService;
        }

        public override async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
        {
            var dbFile = await _dbContext.Files.FirstOrDefaultAsync(file => file.Code == file.Code, cancellationToken);
            if (dbFile is null) 
            {
                throw new NotFoundException(nameof(Domain.File), request.Code);
            }

            // delete from cloud
            var cloudFileName = $"{dbFile.Code}{dbFile.Name.Substring(dbFile.Name.LastIndexOf('.'))}";
            var response = await _storageService.ObjectService.DeleteAsync(cloudFileName);
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
