using Microsoft.EntityFrameworkCore;
using EssTeamApi.Models;

namespace EssTeamApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Manager> Managers { get; set; }
    }
}
