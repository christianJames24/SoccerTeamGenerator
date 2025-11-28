using System.ComponentModel.DataAnnotations;

namespace BalancedSoccerTeam.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(1, 100)]
        public int BallControl { get; set; }

        [Range(1, 100)]
        public int PassingAccuracy { get; set; }

        [Range(1, 100)]
        public int Dribbling { get; set; }

        [Range(1, 100)]
        public int DefensiveAwareness { get; set; }

        [Range(1, 100)]
        public int Shooting { get; set; }

        public double OverallScore { get; set; }

        public int Rank { get; set; }
    }
}