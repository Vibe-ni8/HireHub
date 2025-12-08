using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class Candidate : BaseEntity
{
    public Candidate() : base("CANDIDATE")
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public int Experience { get; set; }
    public string CurrentPosition { get; set; } = null!;
    public string? ResumeUrl { get; set; } = null!;
    public virtual ICollection<CandidateMap> CandidateMaps { get; set; } = [];

}


