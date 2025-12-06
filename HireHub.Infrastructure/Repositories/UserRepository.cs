using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private new readonly HireHubDbContext _context;

    public UserRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string emailId)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.EmailId == emailId);
    }

    public List<CandidateMap> GetCandidatesAssignedForUserOnSlot(int userSlotId)
    {
        return _context.CandidateMaps
            .Where(cm => cm.UserSlotId == userSlotId)
            .Include(cm => cm.Candidate)
            .ToList();
    }

    public async Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId)
    {
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.UserSlots, u => u.Id, us => us.UserId, (u, us) => us)
            .Include(us => us.Slot)
            .Where(us => 
                us.Slot.SlotDate > today || 
                (us.Slot.SlotDate == today && us.Slot.StartTime > currentTime)
             )
            .ToListAsync();
    }
}
