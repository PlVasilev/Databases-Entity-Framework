using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace P03_FootballBetting2OnetoMany.Data.Models
{
   public class Position
    {
        public int PositionId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Player> Players { get; set; } = new HashSet<Player>();
    }
}
