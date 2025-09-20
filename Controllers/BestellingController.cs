using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Data;
using WebAPIDemo.Models;

namespace WebAPIDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingController : ControllerBase
    {
        private readonly WebAPIDemoContext _context;

        public BestellingController(WebAPIDemoContext context)
        {
            _context = context;
        }

        // GET: api/Bestelling
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestelling>>> AlleBestellingenOphalen()
        {
            return await _context.Bestellingen.ToListAsync();
        }

        // GET: api/Bestelling/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestelling>> BestellingOphalen(int id)
        {
            var bestelling = await _context.Bestellingen.FindAsync(id);

            if (bestelling == null)
            {
                return NotFound();
            }

            return bestelling;
        }

        // PUT: api/Bestelling/5
        [HttpPut("{id}")]
        public async Task<IActionResult> BestellingWijzigen(int id, Bestelling bestelling)
        {
            if (id != bestelling.Id)
            {
                return BadRequest("De id's van de bestelling die je wil wijzigen komen niet overeen.");
            }

            _context.Bestellingen.Update(bestelling);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestellingExists(id))
                {
                    return NotFound("Er is een probleem opgetreden bij het opslaan in de database.");
                }
                else
                {
                    throw;
                }
            }

            return Ok($"De bestelling is gewijzigd.");
        }

            // POST: api/Bestelling
        [HttpPost]
        public async Task<ActionResult<Bestelling>> BestellingToevoegen(Bestelling bestelling)
        {
            if (_context.Bestellingen == null)
                return NotFound("De tabel Bestellingen bestaat niet in de database.");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Bestellingen.Add(bestelling);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbError)
            {
                return BadRequest(dbError);
            }

            return CreatedAtAction("BestellingOphalen", new { id = bestelling.Id }, bestelling);
        }

        // DELETE: api/Bestelling/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> BestellingVerwijderen(int id)
        {
             var bestelling = await _context.Bestellingen.FirstOrDefaultAsync(b => b.Id == id);

            if (bestelling == null)
            {
                return NotFound("De bestelling werd niet gevonden.");
            }

            _context.Bestellingen.Remove(bestelling);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Log de fout of geef een aangepaste foutmelding terug
                // Algemene vorm:
                // return StatusCode(StatusCodes.Status500InternalServerError, "Er is een fout opgetreden bij het verwijderen van de bestelling.");
                // Verkorte vorm:
                return StatusCode(500, "Er is een fout opgetreden bij het verwijderen van de bestelling.");
            }

            return Ok($"De bestelling met id {id} is verwijderd.");
        }

        private bool BestellingExists(int id)
        {
            return _context.Bestellingen.Any(e => e.Id == id);
        }
    }
}
