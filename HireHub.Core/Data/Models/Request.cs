using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

using System;

public class Request : BaseEntity
{
    public Request() : base("requests")
    {
    }

    public int RequestId { get; set; }
    public string RequestType { get; set; } = null!;
    public string SubType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public int RequestedBy { get; set; }
    public DateTime RequestedDate { get; set; }

    // Navigation
    public User? Approver { get; set; }
    public User? Requester { get; set; }
}
