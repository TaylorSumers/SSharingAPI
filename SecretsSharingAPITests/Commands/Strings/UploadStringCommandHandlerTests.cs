
using Application.Commands.Strings.Upload;
using Microsoft.Extensions.Configuration;
using SecretsSharingAPITests.Common;
using Microsoft.EntityFrameworkCore;

namespace SecretsSharingAPITests.Commands.Strings
{
    public class UploadStringCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task UploadStringCommandHandler_Success()
        {
            // Arrange
            var config = new ConfigurationManager();
            config["PublicApiBaseUrl"] = "https://host";
            var handler = new UploadStringCommandHandler(_context, config);
            var val = "value";
            var deleteAfterDownload = false;


            // Act
            var strUrl = await handler.Handle(
                new UploadStringCommand
                {
                    Value = val,
                    DeleteAfterDownload = deleteAfterDownload,
                    UserId = SecretsContextFactory.UserAId
                },
                CancellationToken.None);

            // Assert
            var dbStr = await _context.Strings.SingleOrDefaultAsync(str =>
                str.Value == val &&
                str.DeleteAfterDownload == deleteAfterDownload &&
                str.UserId == SecretsContextFactory.UserAId);
            Assert.NotNull(dbStr);
            Assert.Equal(strUrl, $"{config["PublicApiBaseUrl"]}/api/Strings/Get/{dbStr.Code}");

        }
    }
}
