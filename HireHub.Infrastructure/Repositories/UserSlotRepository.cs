using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;

namespace HireHub.Infrastructure.Repositories;

public class UserSlotRepository : GenericRepository<UserSlot>, IUserSlotRepository
{
    private new readonly HireHubDbContext _context;

    public UserSlotRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }

}
