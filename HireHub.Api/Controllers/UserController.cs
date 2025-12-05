using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram;
using HireHub.Shared.Authentication.Filters;
using HireHub.Shared.Common.Exceptions;
using HireHub.Shared.Common.Models;
using HireHub.Shared.Middleware.Models;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Api.Controllers;

[RequireAuth]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("current/info")]
    [ProducesResponseType<UserResponse<UserDetailsDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetCurrentUserInfo()
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCurrentUserInfo));

        try
        {
            var response = await _userService.GetCurrentUserDetails();

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserInfo));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetCurrentUserInfo), ex.Message);
            return BadRequest(new BaseResponse() { Errors = [new { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }] });
        }
    }

    [HttpGet("current/info/all")]
    [ProducesResponseType<UserResponse<UserCompleteDetailsDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetCurrentUserAllInfo()
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCurrentUserAllInfo));

        try
        {
            var response = await _userService.GetCurrentUserCompleteDetails();

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserAllInfo));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetCurrentUserAllInfo), ex.Message);
            return BadRequest(new BaseResponse() { Errors = [new { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }] });
        }
    }
}
