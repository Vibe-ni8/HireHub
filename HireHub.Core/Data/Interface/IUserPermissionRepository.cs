using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserPermissionRepository : IGenericRepository<Role>
{
    Task<UserPermission> GetUserPermissionAsync(int userId, string userAction);
}
