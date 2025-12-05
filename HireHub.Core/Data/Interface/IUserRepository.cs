using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string emailId);
    List<Candidate> GetCandidatesAssignedForUserOnSlot(int userSlotId);
    Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId);
}
