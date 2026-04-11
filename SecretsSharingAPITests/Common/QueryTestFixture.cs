using AutoMapper;
using Persistence;
using Application.Common.Mappings;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace SecretsSharingAPITests.Common
{
    public class QueryTestFixture : IDisposable
    {
        public SecretsDbContext Context { get; }
        public IMapper Mapper { get; }

        public QueryTestFixture()
        {
            Context = SecretsContextFactory.Create();
            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AssemblyMappingProfile(typeof(ISecretsDbContext).Assembly));
            },
            new LoggerFactory());
            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose() 
        {
            SecretsContextFactory.Destroy(Context);
        }

        [CollectionDefinition("QueryCollection")]
        public class QueryCollection : ICollectionFixture<QueryTestFixture> { }
    }
}
