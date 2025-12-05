using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class UserSlot : BaseEntity
{
    public UserSlot() : base("USER_SLOT")
    {
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    public int SlotId { get; set; }
    public virtual Slot Slot { get; set; } = null!;
    public virtual ICollection<CandidateMap> CandidateMaps { get; set; } = null!;
}


