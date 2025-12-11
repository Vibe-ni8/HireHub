using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

using System;
using System.Collections.Generic;

#nullable enable

public class User : BaseEntity
{
    public User() : base("users") 
    {
    }

    public int UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    // Navigation
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
    public ICollection<Drive> CreatedDrives { get; set; } = new List<Drive>();
    public ICollection<DriveMember> DriveMembers { get; set; } = new List<DriveMember>();
    public ICollection<DriveCandidate> RecruitedCandidates { get; set; } = new List<DriveCandidate>();
    public ICollection<Round> InterviewedPanels { get; set; } = new List<Round>();
    public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    public ICollection<CandidateReassignment> RequestedReassignments { get; set; } = new List<CandidateReassignment>();
    public ICollection<CandidateReassignment> ApprovedReassignments { get; set; } = new List<CandidateReassignment>();
}
