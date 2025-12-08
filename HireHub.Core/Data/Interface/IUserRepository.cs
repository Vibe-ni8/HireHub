using HireHub.Core.Data.Models;
using HireHub.Shared.Persistence.Interface;

namespace HireHub.Core.Data.Interface;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string emailId);
    List<CandidateMap> GetCandidatesAssignedForUserOnSlot(int userSlotId);
    Task<List<UserSlot>> GetUserSlotsWithSlotDetailsForUser(int userId);
    Task<bool> IsUserRegisterForTheSlot(int userId, int slotId);
    Task<bool> IsUserRegisterAnyOfTheSlots(int userId, List<int> slotIds);
    Task<List<int>> RegisterUserToSlots(int userId, List<int> slotIds);
}
