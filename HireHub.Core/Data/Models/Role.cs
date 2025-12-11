using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

using System.Collections.Generic;

public class Role : BaseEntity
{
    public Role() : base("roles")
    {
    }

    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<DriveRoleConfiguration> DriveRoleConfigurations { get; set; } = new List<DriveRoleConfiguration>();
    public ICollection<DriveMember> DriveMembers { get; set; } = new List<DriveMember>();
}
