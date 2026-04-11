using Persistence;

namespace SecretsSharingAPITests.Common
{
    public abstract class TestCommandBase : IDisposable
    {
        protected readonly SecretsDbContext _context;

        protected TestCommandBase()
        {
            _context = SecretsContextFactory.Create();
        }

        public void Dispose()
        {
            SecretsContextFactory.Destroy(_context);
        }
    }
}
