using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IInterviewRepository : IGenericRepository<Interview>
{
    Task<int> CountInterviewsAsync(CancellationToken cancellationToken = default);
}