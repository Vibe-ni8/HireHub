using System;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure.Repositories;

public class CandidateRepository : GenericRepository<Candidate>,  ICandidateRepository
{
    private new readonly HireHubDbContext _context;

    public CandidateRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> CountCandidatesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Candidates.CountAsync(cancellationToken);
    }

    public async Task<int> CountByDriveStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.DriveCandidates
            .Where(dc => dc.Status == status)
            .CountAsync(cancellationToken);
    }
}
