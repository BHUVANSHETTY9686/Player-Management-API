using Microsoft.EntityFrameworkCore;
using PlayerManagementAPI.Models;

namespace PlayerManagementAPI.Data
{
    public class PlayerDbContext : DbContext
    {
        public PlayerDbContext(DbContextOptions<PlayerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; }
    }
}
