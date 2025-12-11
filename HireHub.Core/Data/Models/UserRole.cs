using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class UserRole : BaseEntity
{
    public UserRole() : base("user_roles")
    {
    }

    public int UserRoleId { get; set; } // PK
    public int UserId { get; set; }
    public int RoleId { get; set; }

    // Navigation
    public User? User { get; set; }
    public Role? Role { get; set; }
}
