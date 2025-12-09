using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;

namespace HireHub.Infrastructure.Repositories;

public class SlotRepository : GenericRepository<Slot>, ISlotRepository
{
    private new readonly HireHubDbContext _context;

    public SlotRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> IsPastSlot(int slotId, CancellationToken cancellationToken = default)
    {
        var now = DateTime.Now;
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        var slot = await GetByIdAsync(slotId, cancellationToken);
        return slot == null || slot.SlotDate < today || slot.EndTime < currentTime;
    }
}
