using HireHub.Core.Data.Interface;
using HireHub.Core.Service;
using HireHub.Core.Utils.UserProgram.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Api.Controllers;

public class HrController : ControllerBase
{
    private readonly UserService _userService;
    private readonly IUserProvider _userProvider;
    private readonly RepoService _repoService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<UserController> _logger;

    public HrController(UserService userService, IUserProvider userProvider,
        RepoService repoService, ITransactionRepository transactionRepository,
        ILogger<UserController> logger)
    {
        _userService = userService;
        _userProvider = userProvider;
        _repoService = repoService;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }


    #region Get API's



    #endregion

    #region Post API's



    #endregion
}
