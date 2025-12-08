namespace HireHub.Core.DTO;

public class SetFeedbackRequest
{
    public int CandidateId { get; set; }
    public int UserSlotId { get; set; }
    public int StarRating { get; set; }
    public string TechnicalSkills { get; set; } = null!;
    public string CommunicationSkill { get; set; } = null!;
    public string ProblemSolvingAbility { get; set; } = null!;
    public string OverallFeedback { get; set; } = null!;
    public string Recommendation { get; set; } = null!;
}
