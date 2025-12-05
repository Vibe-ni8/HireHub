using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;

namespace HireHub.Infrastructure.Repositories;

public class CandidateRepository : GenericRepository<Candidate>, ICandidateRepository
{
    private new readonly HireHubDbContext _context;

    public CandidateRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }
}
