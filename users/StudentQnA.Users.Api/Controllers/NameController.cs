using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentQnA.Users.Api.Data;
using StudentQnA.Users.Api.Models;

namespace StudentQnA.Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/names
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NameEntity>>> GetNames()
        {
            return await _context.Names.ToListAsync();
        }

        // POST: api/names
        [HttpPost]
        public async Task<ActionResult<NameEntity>> PostName(NameEntity name)
        {
            _context.Names.Add(name);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetNames), new { id = name.Id }, name);
        }
    }
}
