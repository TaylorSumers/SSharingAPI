using Domain;
using Microsoft.EntityFrameworkCore;
using File = Domain.File;
using String = Domain.String;

namespace Application.Interfaces
{
    public interface ISecretsDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<String> Strings { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken); // TODO: чекнуть инфу про cancellationToken
    }
}
