using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class User : BaseEntity
{
    public User() : base("USER")
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string EmailId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? PasswordHash { get; set; }
    public virtual ICollection<UserSlot> UserSlots { get; set; } = [];

}

public class CandidateFeedback
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public int StarRating { get; set;}
    public string TechnicalSkills { get; set; } = null!;
    public string CommunicationSkill { get; set; } = null!;
    public string ProblemSolvingAbility { get; set; } = null!;
    public string OverallFeedback { get; set; } = null!;
    public string Recommendation {  get; set; } = null!;
}

