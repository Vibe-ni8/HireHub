using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string emailId, CancellationToken cancellationToken = default);
    Task<int> CountUsersAsync(CancellationToken cancellationToken = default);
    Task<int> CountUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllWithRoleAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllHrsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllPanelMembersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<List<User>> GetAllMentorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
}
