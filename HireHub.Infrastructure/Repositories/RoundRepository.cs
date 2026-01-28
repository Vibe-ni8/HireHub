using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure.Repositories;

public class RoundRepository : GenericRepository<Round>, IRoundRepository
{
    private new readonly HireHubDbContext _context;

    public RoundRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }


    #region DQL

    public async Task<int> CountInterviewsAsync(RoundStatus? roundStatus,CancellationToken cancellationToken = default)
    {
        var query = _context.Rounds.Select(e => e);
        if (roundStatus != null)
            query = query.Where(r => r.Status == roundStatus);
        return await query.CountAsync(cancellationToken);
    }

    #endregion

    #region DML



    #endregion

    #region Private Methods



    #endregion
}
