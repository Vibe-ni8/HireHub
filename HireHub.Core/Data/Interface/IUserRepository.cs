using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string emailId, CancellationToken cancellationToken = default);
    List<CandidateMap> GetCandidatesAssignedForUserOnSlot(int userSlotId);
    Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId, CancellationToken cancellationToken = default);
    Task<bool> IsUserRegisterForTheSlot(int userId, int slotId, CancellationToken cancellationToken = default);
    Task<bool> IsUserRegisterAnyOfTheSlots(int userId, List<int> slotIds, CancellationToken cancellationToken = default);
    Task<List<int>> RegisterUserToSlots(int userId, List<int> slotIds, CancellationToken cancellationToken = default);
}
