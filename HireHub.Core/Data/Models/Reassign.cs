namespace HireHub.Core.Data.Models;

public class Reassign
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    public int OldUserSlotId { get; set; }
    public virtual UserSlot OldUserSlot { get; set; } = null!;
    public int NewUserSlotId { get; set; }
    public virtual UserSlot NewUserSlot { get; set; } = null!;
    public string Reason { get; set; } = null!;
    public string? AdditionalNotes { get; set; } = null;
}

