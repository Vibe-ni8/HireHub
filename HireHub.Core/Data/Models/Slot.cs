using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class Slot : BaseEntity
{
    public Slot() : base("SLOT")
    {
    }

    public int Id { get; set; }
    public DateOnly SlotDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public virtual ICollection<UserSlot> UserSlots { get; set; } = null!;

}


