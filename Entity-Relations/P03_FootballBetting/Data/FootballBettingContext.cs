using Microsoft.EntityFrameworkCore;
using P03_FootballBetting2OnetoMany.Data.Models;

namespace P03_FootballBetting2OnetoMany.Data
{
    public class FootballBettingContext  : DbContext
    {
        public FootballBettingContext()
        {
        }

        public FootballBettingContext(DbContextOptions<FootballBettingContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Bet> Bets { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Town> Towns { get; set; }
        public virtual DbSet<User> Users { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=DESKTOP-OAP1RAB\\SQLEXPRESS;Database=FootballBettingDB;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(sc => new { sc.PlayerId, sc.GameId });

            modelBuilder.Entity<Game>()
                .HasOne(e => e.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(e => e.AwayTeam)
                .WithMany(t => t.AwayGames)
                .HasForeignKey(e => e.AwayTeamId);


            modelBuilder.Entity<Team>()
                .HasOne(e => e.PrimaryKitColor)
                .WithMany(t => t.PrimaryKitTeams)
                .HasForeignKey(e => e.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasOne(e => e.SecondaryKitColor)
                .WithMany(t => t.SecondaryKitTeams)
                .HasForeignKey(e => e.SecondaryKitColorId);
        }
    }
}
