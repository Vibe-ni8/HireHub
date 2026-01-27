using HireHub.Core.Data.Models;

namespace HireHub.Core.Data.Filters;

public class DriveMemberFilter
{
    public int? DriveId { get; set; }
    public int? UserId { get; set; }
    public UserRole? Role { get; set; }
}
