using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bookRentalShopApi.Models;

namespace bookRentalShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookstblsController : ControllerBase
    {
        private readonly BookrentalshopContext _context;

        public BookstblsController(BookrentalshopContext context)
        {
            _context = context;
        }

        // GET: api/Bookstbls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bookstbl>>> GetBookstbls()
        {
          if (_context.Bookstbls == null)
          {
              return NotFound();
          }
            return await _context.Bookstbls.ToListAsync();
        }

        // GET: api/Bookstbls/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bookstbl>> GetBookstbl(int id)
        {
          if (_context.Bookstbls == null)
          {
              return NotFound();
          }
            var bookstbl = await _context.Bookstbls.FindAsync(id);

            if (bookstbl == null)
            {
                return NotFound();
            }

            return bookstbl;
        }

        // PUT: api/Bookstbls/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBookstbl(int id, Bookstbl bookstbl)
        {
            if (id != bookstbl.BookIdx)
            {
                return BadRequest();
            }

            _context.Entry(bookstbl).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookstblExists(id))
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

        // POST: api/Bookstbls
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bookstbl>> PostBookstbl(Bookstbl bookstbl)
        {
          if (_context.Bookstbls == null)
          {
              return Problem("Entity set 'BookrentalshopContext.Bookstbls'  is null.");
          }
            _context.Bookstbls.Add(bookstbl);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBookstbl", new { id = bookstbl.BookIdx }, bookstbl);
        }

        // DELETE: api/Bookstbls/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookstbl(int id)
        {
            if (_context.Bookstbls == null)
            {
                return NotFound();
            }
            var bookstbl = await _context.Bookstbls.FindAsync(id);
            if (bookstbl == null)
            {
                return NotFound();
            }

            _context.Bookstbls.Remove(bookstbl);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookstblExists(int id)
        {
            return (_context.Bookstbls?.Any(e => e.BookIdx == id)).GetValueOrDefault();
        }
    }
}
