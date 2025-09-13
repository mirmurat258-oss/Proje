using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }

        public void EnsureDatabaseCreated()
        {
            if (this.Database.EnsureCreated())
            {
                SeedData();
            }
        }
        private void SeedData()
        {
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Id = Guid.NewGuid(),
                    Password="admin",
                    Username  ="admin",
                });

                SaveChanges();
            }
        }
    }
}


