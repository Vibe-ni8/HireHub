using HireHub.Core.Data.Interface;

namespace HireHub.Core.Service;

public class RepoService
{
    public readonly IUserRepository UserRepository;

    public RepoService(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }
}
