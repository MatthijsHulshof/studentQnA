using Microsoft.EntityFrameworkCore;
using StudentQnA.Users.Api.Models;

namespace StudentQnA.Users.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NameEntity> Names { get; set; } = null!;
    }
}
