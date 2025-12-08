namespace HireHub.Core.Data.Models;

public class Feedback
{
    public int Id { get; set; }
    public int StarRating { get; set;}
    public string TechnicalSkills { get; set; } = null!;
    public string CommunicationSkill { get; set; } = null!;
    public string ProblemSolvingAbility { get; set; } = null!;
    public string OverallFeedback { get; set; } = null!;
    public string Recommendation {  get; set; } = null!;
    public CandidateMap CandidateMap { get; set; } = null!;
}
