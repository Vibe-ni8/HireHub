using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IRoundRepository : IGenericRepository<Round>
{
    #region DQL

    Task<int> CountInterviewsAsync(RoundStatus? roundStatus, CancellationToken cancellationToken = default);

    #endregion

    #region DML



    #endregion
}