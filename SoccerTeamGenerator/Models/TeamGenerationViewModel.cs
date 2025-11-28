namespace BalancedSoccerTeam.Models
{
    public class TeamGenerationViewModel
    {
        public List<Player> AvailablePlayers { get; set; }
        public List<int> SelectedPlayerIds { get; set; }
        public int NumberOfTeams { get; set; }
    }

    public class TeamResultViewModel
    {
        public List<Team> Teams { get; set; }
    }

    public class Team
    {
        public int TeamNumber { get; set; }
        public List<Player> Players { get; set; }
        public double TotalSkillScore { get; set; }
    }
}