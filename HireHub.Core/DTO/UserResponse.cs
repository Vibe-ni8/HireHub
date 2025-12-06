using HireHub.Shared.Common.Models;

namespace HireHub.Core.DTO;

public class UserResponse<T> : BaseResponse where T : class
{
    public T? Data { get; set; } = null;
}

public class UserDetailsDTO
{
    public int Id { get; set; }
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

public class CandidateDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public TimeOnly ScheduledTime { get; set; }
}

public class UserSlotDetailsDTO
{
    public int Id { get; set; }
    public SlotDetailsDTO Slot { get; set; } = null!;
    public List<CandidateDetailsDTO> Candidates { get; set; } = [];

}

public class UserCompleteDetailsDTO
{
    public UserDetailsDTO User { get; set; } = null!;
    public List<UserSlotDetailsDTO> UserSlots { get; set; } = [];
}
