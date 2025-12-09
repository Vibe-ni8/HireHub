using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface ISlotRepository : IGenericRepository<Slot>
{
    Task<bool> IsPastSlot(int slotId, CancellationToken cancellationToken = default);
}
