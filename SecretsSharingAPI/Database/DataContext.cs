using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretsSharingAPI.Database
{
    public class DataContext : DbContext
    {

        public DbSet<User> User { get; set; }

        public DbSet<File> Files { get; set; }

        public DbSet<FileType> FileType { get; set; }

        public DataContext() => Database.EnsureCreated();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=secrets.db");
        }

    }
}
