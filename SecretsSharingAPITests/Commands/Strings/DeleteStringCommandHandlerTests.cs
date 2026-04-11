using Application.Commands.Strings.Delete;
using Application.Common.Exceptions;
using SecretsSharingAPITests.Common;

namespace SecretsSharingAPITests.Commands.Strings
{
    public class DeleteStringCommandHandlerTests : TestCommandBase
    {
        [Fact]
        public async Task DeleteStringCommandHandler_Success()
        {
            // Arrange
            var handler = new DeleteStringCommandHandler(_context);

            // Act
            await handler.Handle(new DeleteStringCommand
            {
                Code = SecretsContextFactory.StringForDeleteCode
            }, CancellationToken.None);

            // Assert
            Assert.Null(_context.Strings.SingleOrDefault(str => str.Code == SecretsContextFactory.StringForDeleteCode));
        }

        [Fact]
        public async Task DeleteStringCommandHandler_FailsOnWrongId()
        {
            // Arrange
            var handler = new DeleteStringCommandHandler(_context);

            // Act
            // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await handler.Handle(new DeleteStringCommand
                {
                    Code = Guid.NewGuid()
                }, 
                CancellationToken.None));
        }

    }
}
