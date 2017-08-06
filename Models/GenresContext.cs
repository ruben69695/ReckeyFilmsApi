using Microsoft.EntityFrameworkCore;

namespace ReckeyFilmsApi.Models
{
    public class GenresContext : DbContext
    {
        public GenresContext(DbContextOptions<GenresContext> options) : base(options) {}
        public DbSet<Genres> Genres { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Genres>()
                .HasKey(c => new { c.numgenre, c.tmdbId });
        }
    }
}