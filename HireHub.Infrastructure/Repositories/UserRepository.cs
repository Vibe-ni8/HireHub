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

    public List<Candidate> GetCandidatesAssignedForUserOnSlot(int userSlotId)
    {
        return _context.CandidateMaps
            .Where(cm => cm.UserSlotId == userSlotId)
            .Include(cm => cm.Candidate)
            .Select(cm => cm.Candidate)
            .ToList();
    }

    public async Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.UserSlots, u => u.Id, us => us.UserId, (u, us) => us)
            .Include(us => us.Slot)
            .Where(us => new DateTime(us.Slot.SlotDate, TimeOnly.MinValue) > DateTime.Now.AddDays(-1))
            .ToListAsync();
    }
}
