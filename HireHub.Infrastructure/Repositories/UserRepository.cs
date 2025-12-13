using System;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.Utils.Common;
using HireHub.Shared.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace HireHub.Infrastructure.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    private new readonly HireHubDbContext _context;

    public UserRepository(HireHubDbContext context) : base(context)
    {
        _context = context;
    }


    #region DQL

    public async Task<User?> GetByEmailAsync(string emailId, CancellationToken cancellationToken = default)
    {
        return await _context.Users.FirstOrDefaultAsync(e => e.Email == emailId, cancellationToken);
    }

    public async Task<int> CountUsersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Users.CountAsync(cancellationToken);
    }

    public async Task<int> CountUsersByRoleAsync(UserRole role, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(r => r.RoleName == role)
            .Join(_context.Users, r => r.RoleId, u => u.RoleId, (r, u) => u)
            .CountAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllWithRoleAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Include(e => e.Role)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<User>> GetAllHrsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await GetAllInRoleAsync(UserRole.HR, pageNumber, pageSize, cancellationToken);
    }

    public async Task<List<User>> GetAllPanelMembersAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await GetAllInRoleAsync(UserRole.Panel, pageNumber, pageSize, cancellationToken);
    }

    public async Task<List<User>> GetAllMentorsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await GetAllInRoleAsync(UserRole.Mentor, pageNumber, pageSize, cancellationToken);
    }

    #endregion

    #region DML



    #endregion

    #region Private Methods

    private async Task<List<User>> GetAllInRoleAsync(UserRole role, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.Roles
            .Where(r => r.RoleName == role)
            .Join(_context.Users, r => r.RoleId, u => u.RoleId, (r, u) => u)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    #endregion
}
