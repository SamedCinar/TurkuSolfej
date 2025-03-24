using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TurkSolfej.API.Data;
using TurkSolfej.API.Models;

namespace TurkSolfej.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurkulerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurkulerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turku>>> GetTurkuler()
        {
            return await _context.Turkuler.ToListAsync();
        }

        [HttpGet("populer")]
        public async Task<ActionResult<IEnumerable<Turku>>> GetPopulerTurkuler()
        {
            return await _context.Turkuler
                .OrderByDescending(t => t.ViewCount)
                .Take(10)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Turku>> GetTurku(int id)
        {
            var turku = await _context.Turkuler.FindAsync(id);
            if (turku == null)
            {
                return NotFound();
            }
            return turku;
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Turku>>> SearchTurkuler([FromQuery] string q)
        {
            if (string.IsNullOrEmpty(q))
            {
                return BadRequest("Arama terimi boş olamaz");
            }

            return await _context.Turkuler
                .Where(t => t.Title.Contains(q) || 
                           t.Artist.Contains(q) || 
                           t.Region.Contains(q))
                .ToListAsync();
        }

        [HttpGet("{id}/nota")]
        public async Task<ActionResult<Turku>> GetTurkuNota(int id)
        {
            var turku = await _context.Turkuler.FindAsync(id);
            if (turku == null)
            {
                return NotFound();
            }
            return turku;
        }

        [HttpPost]
        public async Task<ActionResult<Turku>> CreateTurku(Turku turku)
        {
            turku.CreatedAt = System.DateTime.Now;
            turku.UpdatedAt = System.DateTime.Now;
            
            _context.Turkuler.Add(turku);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTurku), new { id = turku.Id }, turku);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTurku(int id, Turku turku)
        {
            if (id != turku.Id)
            {
                return BadRequest();
            }

            var existingTurku = await _context.Turkuler.FindAsync(id);
            if (existingTurku == null)
            {
                return NotFound();
            }

            existingTurku.Title = turku.Title;
            existingTurku.Artist = turku.Artist;
            existingTurku.Region = turku.Region;
            existingTurku.ImageUrl = turku.ImageUrl;
            existingTurku.NotaUrl = turku.NotaUrl;
            existingTurku.VideoUrl = turku.VideoUrl;
            existingTurku.Description = turku.Description;
            existingTurku.UpdatedAt = System.DateTime.Now;
            existingTurku.Tags = turku.Tags;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurkuExists(id))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurku(int id)
        {
            var turku = await _context.Turkuler.FindAsync(id);
            if (turku == null)
            {
                return NotFound();
            }

            _context.Turkuler.Remove(turku);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TurkuExists(int id)
        {
            return _context.Turkuler.Any(e => e.Id == id);
        }
    }
} 