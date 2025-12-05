using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;

namespace HireHub.Infrastructure.Repositories;

public class CandidateMapRepository : GenericRepository<CandidateMap>, ICandidateMapRepository
{
    private new readonly HireHubDbContext _context;

    public CandidateMapRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }
}
