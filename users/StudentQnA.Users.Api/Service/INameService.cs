using StudentQnA.Users.Api.Models;

namespace StudentQnA.Users.Api.Service
{
    public interface INameService
    {
        Task<List<NameEntity>> GetAllAsync(CancellationToken ct = default);
        Task<NameEntity> CreateAsync(NameEntity name, CancellationToken ct = default);
    }
}
