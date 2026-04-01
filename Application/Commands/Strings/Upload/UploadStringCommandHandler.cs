using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;

namespace Application.Commands.Strings.Upload
{
    public class UploadStringCommandHandler : HandlerBase<UploadStringCommand, string>
    {
        public UploadStringCommandHandler(ISecretsDbContext dbContext, YandexStorageService storageService) : base(dbContext) { }

        public override async Task<string> Handle(UploadStringCommand request, CancellationToken cancellationToken)
        {
            var dbString = new Domain.String
            {
                Value = request.Value,
                Code = Guid.NewGuid(),
                DeleteAfterDownload = request.DeleteAfterDownload,
                UserId = request.UserId,
            };

            await _dbContext.Strings.AddAsync(dbString, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return $"https://localhost:44306/api/Strings/Get/{dbString.Code}"; // TODO: заменить хардкод
        }
    }
}
