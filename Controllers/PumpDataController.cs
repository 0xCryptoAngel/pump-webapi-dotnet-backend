using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PUMP_BACKEND.Models;
using PUMP_BACKEND.Data;
using Microsoft.AspNetCore.Authorization;

namespace PUMP_BACKEND.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PumpDataController : ControllerBase
    {
        private readonly PumpDbContext _context;

        public PumpDataController(PumpDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pump>>> GetPumps()
        {
            return await _context.Pumps.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Pump>> GetPump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null)
                return NotFound();

            return pump;
        }

        [HttpPost]
        public async Task<ActionResult<Pump>> CreatePump(Pump pump)
        {
            _context.Pumps.Add(pump);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPump), new { id = pump.Id }, pump);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePump(int id, Pump pump)
        {
            if (id != pump.Id)
                return BadRequest();

            _context.Entry(pump).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePump(int id)
        {
            var pump = await _context.Pumps.FindAsync(id);
            if (pump == null)
                return NotFound();

            _context.Pumps.Remove(pump);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
