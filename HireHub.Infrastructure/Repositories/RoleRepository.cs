using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;

namespace HireHub.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    private new readonly HireHubDbContext _context;

    public RoleRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }
}
