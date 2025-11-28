using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BalancedSoccerTeam.Data;
using BalancedSoccerTeam.Models;

namespace BalancedSoccerTeam.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var players = await _context.Players.ToListAsync();
            return View(players);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Player player)
        {
            if (ModelState.IsValid)
            {
                CalculatePlayerStats(player);
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players.FindAsync(id);
            if (player == null) return NotFound();

            return View(player);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Player player)
        {
            if (id != player.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    CalculatePlayerStats(player);
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var player = await _context.Players.FirstOrDefaultAsync(m => m.Id == id);
            if (player == null) return NotFound();

            return View(player);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SelectPlayers()
        {
            var players = await _context.Players.OrderBy(p => p.Rank).ToListAsync();
            var viewModel = new TeamGenerationViewModel
            {
                AvailablePlayers = players,
                SelectedPlayerIds = new List<int>()
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateTeams(List<int> selectedPlayerIds, int numberOfTeams)
        {
            if (selectedPlayerIds == null || selectedPlayerIds.Count == 0)
            {
                TempData["Error"] = "Please select at least one player.";
                return RedirectToAction(nameof(SelectPlayers));
            }

            if (numberOfTeams < 2)
            {
                TempData["Error"] = "Number of teams must be at least 2.";
                return RedirectToAction(nameof(SelectPlayers));
            }

            var selectedPlayers = await _context.Players
                .Where(p => selectedPlayerIds.Contains(p.Id))
                .OrderByDescending(p => p.OverallScore)
                .ToListAsync();

            var teams = BalanceTeams(selectedPlayers, numberOfTeams);

            var viewModel = new TeamResultViewModel { Teams = teams };
            return View(viewModel);
        }

        private void CalculatePlayerStats(Player player)
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

        private List<Team> BalanceTeams(List<Player> players, int numberOfTeams)
        {
            var teams = new List<Team>();
            for (int i = 0; i < numberOfTeams; i++)
            {
                teams.Add(new Team
                {
                    TeamNumber = i + 1,
                    Players = new List<Player>()
                });
            }

            int currentTeam = 0;
            bool reverse = false;

            foreach (var player in players)
            {
                teams[currentTeam].Players.Add(player);

                if (!reverse)
                {
                    currentTeam++;
                    if (currentTeam >= numberOfTeams)
                    {
                        currentTeam = numberOfTeams - 1;
                        reverse = true;
                    }
                }
                else
                {
                    currentTeam--;
                    if (currentTeam < 0)
                    {
                        currentTeam = 0;
                        reverse = false;
                    }
                }
            }

            foreach (var team in teams)
            {
                team.TotalSkillScore = team.Players.Sum(p => p.OverallScore);
            }

            return teams;
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}