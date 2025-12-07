using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;
using HireHub.Core.Validators;
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
    private readonly IUserProvider _userProvider;
    private readonly RepoService _repoService;
    private readonly ILogger<UserController> _logger;

    public UserController(UserService userService, IUserProvider userProvider,
        RepoService repoService, ILogger<UserController> logger)
    {
        _userService = userService;
        _userProvider = userProvider;
        _repoService = repoService;
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
            var response = await _userService.GetUserDetails(int.Parse(_userProvider.CurrentUserId));

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserInfo));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetCurrentUserInfo), ex.Message);
            return BadRequest( new BaseResponse() { 
                Errors = [ 
                    new ValidationError { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }
                ] 
            });
        }
    }

    [RequireAuth([Role.Panel])]
    [HttpGet("current/info/all")]
    [ProducesResponseType<UserResponse<UserCompleteDetailsDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetCurrentUserAllInfo()
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCurrentUserAllInfo));

        try
        {
            var response = await _userService.GetUserCompleteDetails(int.Parse(_userProvider.CurrentUserId));

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserAllInfo));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetCurrentUserAllInfo), ex.Message);
            return BadRequest( new BaseResponse() { 
                Errors = [ 
                    new ValidationError { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }
                ] 
            });
        }
    }

    [RequireAuth([Role.Panel])]
    [HttpPost("current/availability/set")]
    [ProducesResponseType<UserResponse<List<string>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> SetAvailability([FromBody] List<string> request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(SetAvailability));

        try
        {
            var baseResponse = new BaseResponse();

            var validator = await new 
                SetAvailabilityRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                .ValidateAsync(request);

            if (!validator.IsValid)
            {
                validator.Errors.ForEach( e => 
                    baseResponse.Errors.Add( new ValidationError { 
                        PropertyName = e.PropertyName, 
                        ErrorMessage = e.ErrorMessage 
                    })
                );
                return Ok(baseResponse);
            }

            var userId = int.Parse(_userProvider.CurrentUserId);
            var slotIds = request.Select(int.Parse).ToList();

            var response = await _userService.SetAvailability(userId, slotIds);

            baseResponse.Warnings.ForEach(response.Warnings.Add);

            _logger.LogInformation(LogMessage.EndMethod, nameof(SetAvailability));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(SetAvailability), ex.Message);
            return BadRequest( new BaseResponse() { 
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Exception, ErrorMessage = ex.Message }
                ] 
            });
        }
    }
}
