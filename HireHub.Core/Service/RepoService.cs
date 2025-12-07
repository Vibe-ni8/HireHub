using HireHub.Core.Data.Interface;

namespace HireHub.Core.Service;

public class RepoService
{
    public readonly IUserRepository UserRepository;
    public readonly ISlotRepository SlotRepository;

    public RepoService(IUserRepository userRepository, ISlotRepository slotRepository)
    {
        UserRepository = userRepository;
        SlotRepository = slotRepository;
    }
}
