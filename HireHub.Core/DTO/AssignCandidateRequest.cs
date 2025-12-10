namespace HireHub.Core.DTO;

public class AssignCandidateRequest
{
    public int CandidateId { get; set; }
    public int UserSlotId { get; set; }
    public int InterviewRounds { get; set; }
}
