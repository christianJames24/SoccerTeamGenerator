using BalancedSoccerTeam.Models;

namespace BalancedSoccerTeam.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Players.Any())
            {
                return;
            }

            var players = new Player[]
            {
                new Player { Name = "Lionel Martinez", BallControl = 95, PassingAccuracy = 92, Dribbling = 96, DefensiveAwareness = 45, Shooting = 94 },
                new Player { Name = "Cristiano Silva", BallControl = 88, PassingAccuracy = 82, Dribbling = 90, DefensiveAwareness = 38, Shooting = 95 },
                new Player { Name = "Kevin Anderson", BallControl = 90, PassingAccuracy = 94, Dribbling = 88, DefensiveAwareness = 65, Shooting = 85 },
                new Player { Name = "Virgil Thompson", BallControl = 75, PassingAccuracy = 78, Dribbling = 65, DefensiveAwareness = 96, Shooting = 62 },
                new Player { Name = "Sergio Walker", BallControl = 72, PassingAccuracy = 80, Dribbling = 68, DefensiveAwareness = 94, Shooting = 58 },
                new Player { Name = "Mohamed Jackson", BallControl = 92, PassingAccuracy = 85, Dribbling = 94, DefensiveAwareness = 42, Shooting = 88 },
                new Player { Name = "Robert Harris", BallControl = 85, PassingAccuracy = 88, Dribbling = 82, DefensiveAwareness = 55, Shooting = 92 },
                new Player { Name = "Luka Clark", BallControl = 88, PassingAccuracy = 92, Dribbling = 86, DefensiveAwareness = 62, Shooting = 80 },
                new Player { Name = "Kylian Lewis", BallControl = 90, PassingAccuracy = 78, Dribbling = 95, DefensiveAwareness = 40, Shooting = 86 },
                new Player { Name = "Erling Robinson", BallControl = 75, PassingAccuracy = 68, Dribbling = 78, DefensiveAwareness = 35, Shooting = 96 },
                new Player { Name = "Harry Young", BallControl = 82, PassingAccuracy = 85, Dribbling = 80, DefensiveAwareness = 50, Shooting = 88 },
                new Player { Name = "Bruno King", BallControl = 86, PassingAccuracy = 90, Dribbling = 84, DefensiveAwareness = 58, Shooting = 82 },
                new Player { Name = "Thibaut Wright", BallControl = 68, PassingAccuracy = 75, Dribbling = 52, DefensiveAwareness = 88, Shooting = 45 },
                new Player { Name = "Alisson Scott", BallControl = 65, PassingAccuracy = 78, Dribbling = 48, DefensiveAwareness = 90, Shooting = 42 },
                new Player { Name = "Joshua Green", BallControl = 78, PassingAccuracy = 82, Dribbling = 75, DefensiveAwareness = 68, Shooting = 72 },
                new Player { Name = "David Adams", BallControl = 80, PassingAccuracy = 76, Dribbling = 72, DefensiveAwareness = 70, Shooting = 75 },
                new Player { Name = "Marcus Baker", BallControl = 70, PassingAccuracy = 72, Dribbling = 68, DefensiveAwareness = 65, Shooting = 70 },
                new Player { Name = "James Nelson", BallControl = 65, PassingAccuracy = 68, Dribbling = 62, DefensiveAwareness = 72, Shooting = 65 },
                new Player { Name = "Luke Carter", BallControl = 60, PassingAccuracy = 65, Dribbling = 58, DefensiveAwareness = 75, Shooting = 60 },
                new Player { Name = "Mason Mitchell", BallControl = 55, PassingAccuracy = 60, Dribbling = 52, DefensiveAwareness = 68, Shooting = 55 }
            };

            foreach (var player in players)
            {
                CalculatePlayerStats(player);
            }

            context.Players.AddRange(players);
            context.SaveChanges();
        }

        private static void CalculatePlayerStats(Player player)
        {
            player.OverallScore = (player.BallControl + player.PassingAccuracy +
                                  player.Dribbling + player.DefensiveAwareness +
                                  player.Shooting) / 5.0;

            if (player.OverallScore >= 80)
                player.Rank = 1;
            else if (player.OverallScore >= 60)
                player.Rank = 2;
            else if (player.OverallScore >= 40)
                player.Rank = 3;
            else if (player.OverallScore >= 20)
                player.Rank = 4;
            else
                player.Rank = 5;
        }
    }
}