using Microsoft.EntityFrameworkCore;
using EssTeamApi.Models;

namespace EssTeamApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Player> Players { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<PlayerTraining> PlayerTrainings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Composite key for join table
            modelBuilder.Entity<PlayerTraining>()
                .HasKey(pt => new { pt.PlayerId, pt.TrainingId });
        }
    }
}
