using HireHub.Core.Data.Models;
using HireHub.Shared.Common.Models;

namespace HireHub.Core.DTO;

public class UserResponse<T> : BaseResponse where T : class
{
    public T? Data { get; set; } = null;
}

public class UserDetailsDTO
{
    public int UserId { get; set; }
    public string Name { get; set; } = null!;
    public string EmailId { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public bool IsActive { get; set; }
    public string? PasswordHash { get; set; }
}

public class SlotDetailsDTO
{
    public DateOnly SlotDate { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}

public class FeedbackDTO
{
    public int Id { get; set; }
    public int StarRating { get; set; }
    public string TechnicalSkills { get; set; } = null!;
    public string CommunicationSkill { get; set; } = null!;
    public string ProblemSolvingAbility { get; set; } = null!;
    public string OverallFeedback { get; set; } = null!;
    public string Recommendation { get; set; } = null!;
}

public class CandidateDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int Experience { get; set; }
    public string CurrentPosition { get; set; } = null!;
    public string? ResumeUrl { get; set; } = null!;
    public TimeOnly ScheduledTime { get; set; }
    public bool? IsPresent { get; set; } = null;
    public int InterviewRounds { get; set; }
    public int? FeedbackId { get; set; }
    public FeedbackDTO? Feedback { get; set; } = null;
    public bool? IsSelected { get; set; } = null!;
}

public class UserSlotDetailsDTO
{
    public int Id { get; set; }
    public SlotDetailsDTO Slot { get; set; } = null!;
    public bool IsLocked { get; set; }
    public List<CandidateDetailsDTO> Candidates { get; set; } = [];

}

public class UserCompleteDetailsDTO
{
    public UserDetailsDTO User { get; set; } = null!;
    public List<UserSlotDetailsDTO> UserSlots { get; set; } = [];
}
