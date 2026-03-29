using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Strings.Delete
{
    public class DeleteStringCommandHandler : HandlerBase<DeleteStringCommand>
    {
        private readonly YandexStorageService _storageService; // TODO: заменить на интерфейс

        public DeleteStringCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService) : base(dbContext)
        {
            _storageService = storageService;
        }

        public async override Task Handle(DeleteStringCommand request, CancellationToken cancellationToken) // TODO: Обработка исключений
        {
            var dbStr = await _dbContext.Strings.FirstOrDefaultAsync(str => str.Code == str.Code, cancellationToken);
            _dbContext.Strings.Remove(dbStr);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
