using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class Slot : BaseEntity
{
    public Slot() : base("SLOT")
    {
    }

    public int Id { get; set; }
    public DateOnly SlotDate { get; set; }
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public virtual ICollection<UserSlot> UserSlots { get; set; } = null!;

}


