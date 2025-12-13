using HireHub.Core.Data.Interface;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class HrService
{
    private readonly IUserRepository _userRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly ILogger<HrService> _logger;

    public HrService(IUserRepository userRepository,
        ISaveRepository saveRepository, ILogger<HrService> logger)
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
