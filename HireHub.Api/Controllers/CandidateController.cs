using HireHub.Core.Data.Interface;
using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;
using HireHub.Shared.Authentication.Filters;
using HireHub.Shared.Common.Exceptions;
using HireHub.Shared.Common.Models;
using HireHub.Shared.Middleware.Models;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Api.Controllers;

[RequireAuth]
[Route("api/[controller]")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly CandidateService _candidateService;
    private readonly IUserProvider _userProvider;
    private readonly RepoService _repoService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<CandidateController> _logger;

    public CandidateController(CandidateService candidateService, IUserProvider userProvider,
        RepoService repoService, ITransactionRepository transactionRepository,
        ILogger<CandidateController> logger)
    {
        _candidateService = candidateService;
        _userProvider = userProvider;
        _repoService = repoService;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }

    #region Get API's

    [RequireAuth([RoleName.Admin])]
    [HttpGet("all")]
    [ProducesResponseType<Response<List<CandidateDTO>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetAllCandidates([FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetAllCandidates));

        try
        {
            var response = await _candidateService.GetAllCandidates(pageNumber, pageSize);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetAllCandidates));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetAllCandidates), ex.Message);
            return BadRequest(new BaseResponse()
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion

    #region Post API's



    #endregion
}
