using HireHub.Core.Data.Models;
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
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
}

public class CandidateDetailsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class UserSlotDetailsDTO
{
    public int Id { get; set; }
    public SlotDetailsDTO Slot { get; set; } = null!;
    public List<CandidateDetailsDTO> Candidates = new List<CandidateDetailsDTO>();

}

public class UserCompleteDetailsDTO
{
    public UserDetailsDTO? User { get; set; } = null!;
    public List<UserSlotDetailsDTO> UserSlots { get; set; } = new List<UserSlotDetailsDTO>();
}
