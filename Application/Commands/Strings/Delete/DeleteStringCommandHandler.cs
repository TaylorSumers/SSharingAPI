using Application.Common.Exceptions;
using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.Strings.Delete
{
    public class DeleteStringCommandHandler : HandlerBase<DeleteStringCommand>
    {
        public DeleteStringCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService) : base(dbContext) { }

        public async override Task Handle(DeleteStringCommand request, CancellationToken cancellationToken)
        {
            var dbStr = await _dbContext.Strings.FirstOrDefaultAsync(str => str.Code == request.Code, cancellationToken);
            if (dbStr is null)
            {
                throw new NotFoundException(nameof(Domain.String), request.Code);
            }
            _dbContext.Strings.Remove(dbStr);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
