namespace Persistence
{
    public class DbInitializer
    {
        public static void Initialize(SecretsDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
