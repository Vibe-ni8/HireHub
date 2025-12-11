using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class PanelVisibilitySettings : BaseEntity
{
    public PanelVisibilitySettings() : base("panel_visibility_settings")
    {
    }

    public int VisibilityId { get; set; }
    public int DriveId { get; set; }
    public bool ShowPhone { get; set; }
    public bool ShowEmail { get; set; }
    public bool ShowPreviousCompany { get; set; }
    public bool ShowResume { get; set; }
    public bool ShowCollege { get; set; }
    public bool ShowAddress { get; set; }
    public bool ShowLinkedIn { get; set; }
    public bool ShowGitHub { get; set; }

    // Navigation
    public Drive? Drive { get; set; }
}
