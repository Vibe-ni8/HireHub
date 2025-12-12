using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string emailId, CancellationToken cancellationToken = default);
    Task<int> CountUsersAsync(CancellationToken cancellationToken = default);
    Task<int> CountUsersByRoleAsync(string roleName, CancellationToken cancellationToken = default);
}
