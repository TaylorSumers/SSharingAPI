using Application.Interfaces;
using AspNetCore.Yandex.ObjectStorage;
using Microsoft.Extensions.Configuration;

namespace Application.Commands.Strings.Upload
{
    public class UploadStringCommandHandler : HandlerBase<UploadStringCommand, string>
    {
        private readonly IConfiguration _configuration;
        public UploadStringCommandHandler(ISecretsDbContext dbContext, IConfiguration configuration) : base(dbContext) 
        {
            _configuration = configuration;
        }

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

            return $"{_configuration["PublicApiBaseUrl"]}/api/Strings/Get/{dbString.Code}";
        }
    }
}
