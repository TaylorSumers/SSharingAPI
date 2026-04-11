using Application.Queries.Strings.Get;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
using SecretsSharingAPITests.Common;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretsSharingAPITests.Queries.Strings
{
    [Collection("QueryCollection")]
    public class GetStringQueryHandlerTests
    {
        private SecretsDbContext _context;
        private IMapper _mapper;

        public GetStringQueryHandlerTests(QueryTestFixture fixture)
        {
            _context = fixture.Context;
            _mapper = fixture.Mapper;
        }

        [Fact]
        public async Task GetStringQueryHandler_Success()
        {
            // Arrange
            var config = new ConfigurationManager();
            config["PublicApiBaseUrl"] = "https://host";
            var handler = new GetStringQueryHandler(_context, _mapper, config);

            // Act
            var result = await handler.Handle(
                new GetStringQuery
                {
                    Code = SecretsContextFactory.StringForGetCode
                },
                CancellationToken.None);

            // Assert
            result.ShouldBeOfType<StringVm>();
            result.Value.ShouldBe("string for get");
            result.DeleteAfterDownload.ShouldBeFalse();
            result.GetUrl.ShouldBe($"{config["PublicApiBaseUrl"]}/api/Strings/Get/{result.Code}");
        }
    }
}
