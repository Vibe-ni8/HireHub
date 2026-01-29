using HireHub.Core.Data.Filters;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IRoundRepository : IGenericRepository<Round>
{
    #region DQL

    Task<int> CountInterviewsAsync(RoundStatus? roundStatus, CancellationToken cancellationToken = default);

    [Obsolete]
    Task<List<Round>> GetAllAsync(RoundFilter filter, CancellationToken cancellationToken = default);

    Task<List<RoundDTO>> GetAllAsDtoAsync(RoundFilter filter, CancellationToken cancellationToken = default);

    Task<RoundDTO?> GetByIdAsDtoAsync(int? roundId, CancellationToken cancellationToken = default);

    bool IsInterviewerForHrRoundForCandidate(int userId, int driveId, int candidateId);

    #endregion

    #region DML



    #endregion
}