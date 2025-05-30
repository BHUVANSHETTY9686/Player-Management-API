using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlayerManagementAPI.Data;
using PlayerManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace PlayerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]  // Require authentication for all actions
    public class PlayersController : ControllerBase
    {
        private readonly PlayerDbContext _context;

        public PlayersController(PlayerDbContext context)
        {
            _context = context;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        // GET: api/Players/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);

            if (player == null)
            {
                return NotFound();
            }

            return player;
        }

        // POST: api/Players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }

        // PUT: api/Players/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Players/seed
        [HttpPost("seed")]
        public async Task<ActionResult<IEnumerable<Player>>> SeedData()
        {
            var players = new List<Player>
            {
                new Player { 
                    Name = "Lionel Messi", 
                    Age = 35, 
                    Position = "Forward", 
                    Team = "Inter Miami", 
                    JoinDate = DateTime.Now.AddYears(-1), 
                    JerseyNumber = 10 
                },
                new Player { 
                    Name = "Cristiano Ronaldo", 
                    Age = 38, 
                    Position = "Forward", 
                    Team = "Al Nassr", 
                    JoinDate = DateTime.Now.AddMonths(-6), 
                    JerseyNumber = 7 
                },
                new Player { 
                    Name = "Erling Haaland", 
                    Age = 22, 
                    Position = "Forward", 
                    Team = "Manchester City", 
                    JoinDate = DateTime.Now.AddYears(-2), 
                    JerseyNumber = 9 
                }
            };

            _context.Players.AddRange(players);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayers), players);
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
