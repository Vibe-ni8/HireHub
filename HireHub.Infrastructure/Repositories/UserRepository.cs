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

    public async Task<User?> GetByEmailAsync(string emailId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.EmailId == emailId, cancellationToken);
    }

    public List<CandidateMap> GetCandidatesAssignedForUserOnSlot(int userSlotId)
    {
        return _context.CandidateMaps
            .Where(cm => cm.UserSlotId == userSlotId)
            .Include(cm => cm.Candidate)
            .Include(cm => cm.Feedback)
            .ToList();
    }

    public async Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.UserSlots, u => u.Id, us => us.UserId, (u, us) => us)
            .Include(us => us.Slot)
            .Where(us => us.Slot.SlotDate >= today )
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsUserRegisterForTheSlot(int userId, int slotId, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.UserSlots, u => u.Id, us => us.UserId, (u, us) => us)
            .AnyAsync(us => us.SlotId == slotId, cancellationToken);
    }

    public async Task<bool> IsUserRegisterAnyOfTheSlots(int userId, List<int> slotIds, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Where(u => u.Id == userId)
            .Join(_context.UserSlots, u => u.Id, us => us.UserId, (u, us) => us)
            .AnyAsync(us => slotIds.Contains(us.SlotId), cancellationToken);
    }

    public async Task<List<int>> RegisterUserToSlots(int userId, List<int> slotIds, CancellationToken cancellationToken = default)
    {
        var user = await _context.Users.FirstAsync(u => u.Id == userId, cancellationToken);
        slotIds.ForEach( slotId => 
            user.UserSlots.Add(new() { UserId = userId, SlotId = slotId, IsLocked = false })
        );
        _context.Update(user);
        _context.SaveChanges();
        return user.UserSlots.Select(us => us.Id).ToList();
    }
}
