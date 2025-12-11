using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class DriveRoleConfiguration : BaseEntity
{
    public DriveRoleConfiguration() : base("drive_role_configuration")
    {
    }

    public int ConfigId { get; set; }
    public int DriveId { get; set; }
    public int RoleId { get; set; }
    public bool AllowPanelReassign { get; set; }
    public bool CanViewFeedback { get; set; }
    public bool AllowBulkUpload { get; set; }
    public bool CanEditSubmittedFeedback { get; set; }
    public bool RequireApprovalForReassignment { get; set; }

    // Navigation
    public Drive? Drive { get; set; }
    public Role? Role { get; set; }
}
