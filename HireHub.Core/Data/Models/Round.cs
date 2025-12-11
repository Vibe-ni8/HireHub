using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class Round : BaseEntity
{
    public Round() : base("rounds")
    {
    }

    public int RoundId { get; set; }
    public int DriveCandidateId { get; set; }
    public int InterviewerId { get; set; }
    public string RoundType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Result { get; set; } = null!;

    // Navigation
    public DriveCandidate? DriveCandidate { get; set; }
    public User? Interviewer { get; set; }
}
