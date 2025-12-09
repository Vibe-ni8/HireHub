using HireHub.Core.Data.Interface;

namespace HireHub.Core.Service;

public class RepoService
{
    public readonly IUserRepository UserRepository;
    public readonly ISlotRepository SlotRepository;
    public readonly ICandidateMapRepository CandidateMapRepository;
    public readonly ICandidateRepository CandidateRepository;
    public readonly IUserSlotRepository UserSlotRepository;

    public RepoService(IUserRepository userRepository, ISlotRepository slotRepository,
        ICandidateMapRepository candidateMapRepository, ICandidateRepository candidateRepository,
        IUserSlotRepository userSlotRepository)
    {
        UserRepository = userRepository;
        SlotRepository = slotRepository;
        CandidateMapRepository = candidateMapRepository;
        CandidateRepository = candidateRepository;
        UserSlotRepository = userSlotRepository;
    }
}
