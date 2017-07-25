using Microsoft.EntityFrameworkCore;

namespace ReckeyFilmsApi.Models
{
    public class GenresContext : DbContext
    {
        public GenresContext(DbContextOptions<GenresContext> options) : base(options)
        {

        }

        public DbSet<ReckeyFilmsApi.Models.Genres> Genres { get; set; }
    }
}