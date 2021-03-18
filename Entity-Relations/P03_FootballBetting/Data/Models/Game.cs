using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P03_FootballBetting2OnetoMany.Data.Models
{
    public class Game
    {
        public int GameId { get; set; }
        
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public int HomeTeamGoals { get; set; }

        public int AwayTeamGoals { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        public decimal HomeTeamBetRate { get; set; }

        public decimal AwayTeamBetRate { get; set; }

        public decimal DrawBetRate { get; set; }

        [Required]
        public string Result { get; set; }

        [ForeignKey(nameof(HomeTeamId))]
        public Team HomeTeam { get; set; }

        [ForeignKey(nameof(AwayTeamId))]
        public Team AwayTeam { get; set; }

        public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
    }
}
