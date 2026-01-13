using Microsoft.EntityFrameworkCore;
using StudentQnA.Users.Api.Data;
using StudentQnA.Users.Api.Models;

namespace StudentQnA.Users.Api.Service
{
    public class NameService : INameService
    {
        private readonly AppDbContext _context;

        public NameService(AppDbContext context)
        {
            _context = context;
        }

        public Task<List<NameEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return _context.Names.ToListAsync(ct);
        }

        public async Task<NameEntity> CreateAsync(NameEntity name, CancellationToken ct = default)
        {
            name.CreatedAt = DateTime.UtcNow;

            _context.Names.Add(name);

            await _context.SaveChangesAsync();

            return name;
        }
    }
}
