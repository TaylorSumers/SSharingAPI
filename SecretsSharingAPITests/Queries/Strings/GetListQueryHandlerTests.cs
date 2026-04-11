using Application.Queries.Strings.GetList;
using AutoMapper;
using Persistence;
using SecretsSharingAPITests.Common;
using Shouldly;


namespace SecretsSharingAPITests.Queries.Strings
{
    [Collection("QueryCollection")]
    public class GetListQueryHandlerTests
    {
        private SecretsDbContext _context;
        private IMapper _mapper;

        public GetListQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetListQueryHandler_Success()
        {
            // Arrange
            var handler = new GetListQueryHandler(_context, _mapper);

            // Act
            var result = await handler.Handle(new GetListQuery
            {
                UserId = SecretsContextFactory.UserAId
            },
            CancellationToken.None);

            // Assert
            result.ShouldBeOfType<List<StringLookupDto>>();
            result.Count.ShouldBe(2);

        }
    }
}
