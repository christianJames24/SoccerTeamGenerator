using Microsoft.EntityFrameworkCore;
using BalancedSoccerTeam.Models;

namespace BalancedSoccerTeam.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}