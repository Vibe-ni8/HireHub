using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class CandidateMap : BaseEntity
{
    public CandidateMap() : base("CANDIDATE_MAP")
    {
    }

    public int CandidateId { get; set; }
    public virtual Candidate Candidate { get; set; } = null!;
    public int UserSlotId { get; set; }
    public virtual UserSlot UserSlot { get; set; } = null!;
    public TimeOnly ScheduledTime { get; set; }
    public bool? IsPresent { get; set; } = null;
    public int InterviewRounds { get; set; }
    public int? FeedbackId { get; set; }
    public Feedback? Feedback { get; set; } = null;

}


