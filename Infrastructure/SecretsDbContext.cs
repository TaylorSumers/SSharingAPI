using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using File = Domain.File;
using String = Domain.String;

namespace Persistence
{
    public class SecretsDbContext : DbContext, ISecretsDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<String> Strings { get; set; }

        public SecretsDbContext(DbContextOptions<SecretsDbContext> options) : base(options) { } // TODO: Зачем нужен параметр
    }
}
