using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
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
    public async Task<IActionResult> GetCandidates([FromQuery] string? experienceLevel,
        [FromQuery] bool isLatestFirst, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate,
        [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCandidates));

        try
        {
            object? expLevel = null;
            if (experienceLevel != null && !Enum.TryParse(typeof(CandidateExperienceLevel), experienceLevel, true, out expLevel))
                throw new CommonException(ResponseMessage.InvalidExperienceLevel);

            var response = await _candidateService.GetCandidates(
                expLevel != null ? (CandidateExperienceLevel)expLevel : null, 
                isLatestFirst, startDate, endDate, pageNumber, pageSize);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetCandidates));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetCandidates), ex.Message);
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion

    #region Post API's



    #endregion
}
