using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NotesOTG_Server.Models.Contexts
{
    public class DatabaseContext : IdentityDbContext<NotesUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Notes> Notes { get; set; }
    }
}