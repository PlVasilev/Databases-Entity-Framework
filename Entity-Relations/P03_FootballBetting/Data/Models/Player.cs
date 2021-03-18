using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting2OnetoMany.Data.Models
{
   public class Player
    {
        public int PlayerId { get; set; }

        [Required]
        public string Name { get; set; }

        public int SquadNumber { get; set; }

        public int TeamId { get; set; }

        public int PositionId { get; set; }

        [Required]
        public bool IsInjured { get; set; }

        [ForeignKey(nameof(TeamId))]
        public Team Team { get; set; }

        [ForeignKey(nameof(PositionId))]
        public Position Position { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
    }
}
