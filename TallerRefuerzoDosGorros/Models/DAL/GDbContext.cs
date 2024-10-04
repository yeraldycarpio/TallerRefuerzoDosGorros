using Microsoft.EntityFrameworkCore;
using TallerRefuerzoDosGorros.Models.EN;

namespace TallerRefuerzoDosGorros.Models.DAL
{
    public class GDbContext : DbContext
    {
        public GDbContext(DbContextOptions<GDbContext> options) : base(options)
        {
        }

        public DbSet<Gorro> Gorros { get; set; }
    }
}
