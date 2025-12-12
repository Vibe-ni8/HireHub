using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface ICandidateRepository : IGenericRepository<Candidate>
{
    Task<int> CountCandidatesAsync(CancellationToken cancellationToken = default);
    Task<int> CountByDriveStatusAsync(string status, CancellationToken cancellationToken = default);
}