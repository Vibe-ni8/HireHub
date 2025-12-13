using HireHub.Core.Data.Interface;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class PanelMemberService
{
    private readonly IUserRepository _userRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly ILogger<CandidateService> _logger;

    public PanelMemberService(IUserRepository userRepository,
        ISaveRepository saveRepository, ILogger<CandidateService> logger)
    {
        _userRepository = userRepository;
        _saveRepository = saveRepository;
        _logger = logger;
    }


    #region Query Services



    #endregion

    #region Command Services



    #endregion

    #region Private Methods



    #endregion
}
