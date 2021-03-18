using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting2OnetoMany.Data.Models
{
    public class Team
    {
        public int TeamId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LogoUrl { get; set; }

        [Required]
        public string Initials { get; set; }

        public decimal Budget { get; set; }

        public int PrimaryKitColorId { get; set; }

        public int SecondaryKitColorId { get; set; }

        public int TownId { get; set; }

        [ForeignKey(nameof(PrimaryKitColorId))]
        public Color PrimaryKitColor { get; set; }

        [ForeignKey(nameof(SecondaryKitColorId))]
        public Color SecondaryKitColor { get; set; }

        [ForeignKey(nameof(TownId))]
        public Town Town { get; set; }

        public ICollection<Game> HomeGames { get; set; } = new HashSet<Game>();

        public ICollection<Game> AwayGames { get; set; } = new HashSet<Game>();

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }
}
