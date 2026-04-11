using Microsoft.EntityFrameworkCore;
using Persistence;

namespace SecretsSharingAPITests.Common
{
    public class SecretsContextFactory
    {
        public static int UserAId = 1;
        public static int UserBId = 2;

        public static Guid StringForDeleteCode = Guid.NewGuid();
        public static Guid StringForGetCode = Guid.NewGuid();

        public static SecretsDbContext Create()
        {
            var options = new DbContextOptionsBuilder<SecretsDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var context = new SecretsDbContext(options);
            context.Database.EnsureCreated();
            context.Files.AddRange(
                new Domain.File
                {
                    Name = "file1",
                    Code = Guid.NewGuid(),
                    DeleteAfterDownload = false,
                    UserId = UserBId
                },
                new Domain.File
                {
                    Name = "file2",
                    Code = Guid.NewGuid(),
                    DeleteAfterDownload = false,
                    UserId = UserBId
                }
                );
            context.Strings.AddRange(
                new Domain.String
                {
                    Value = "string for delete",
                    Code = StringForDeleteCode,
                    DeleteAfterDownload = false,
                    UserId = UserAId,
                },
                new Domain.String
                {
                    Value = "string for get",
                    Code = StringForGetCode,
                    DeleteAfterDownload = false,
                    UserId = UserAId,
                }
                );
            context.SaveChanges();
            return context;
        }

        public static void Destroy(SecretsDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
