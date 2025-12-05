using HireHub.Shared.Common.Models;

namespace HireHub.Core.Data.Models;

public class Candidate : BaseEntity
{
    public Candidate() : base("CANDIDATE")
    {
    }

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public virtual ICollection<CandidateMap> CandidateMaps { get; set; } = null!;

}


