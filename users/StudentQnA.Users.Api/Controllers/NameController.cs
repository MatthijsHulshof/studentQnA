using Microsoft.AspNetCore.Mvc;
using StudentQnA.Users.Api.Models;
using StudentQnA.Users.Api.Service;

namespace StudentQnA.Users.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NamesController : ControllerBase
    {
        private readonly INameService _service;

        public NamesController(INameService service)
        {
            _service = service;
        }

        // GET: api/names
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NameEntity>>> GetNames(CancellationToken ct)
        {
            var names = await _service.GetAllAsync(ct);

            return Ok(names);
        }

        // POST: api/names
        [HttpPost]
        public async Task<ActionResult<NameEntity>> PostName([FromBody] NameEntity name, CancellationToken ct)
        {
            var created = await _service.CreateAsync(name, ct);

            return CreatedAtAction(nameof(GetNames), new { id = created.Id }, created);
        }
    }
}
