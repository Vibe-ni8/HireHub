using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Validators;
using HireHub.Shared.Common.Exceptions;
using HireHub.Shared.Common.Models;
using HireHub.Shared.Middleware.Models;
using Microsoft.AspNetCore.Mvc;

namespace HireHub.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(TokenService tokenService, ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    [HttpPost("token")]
    [ProducesResponseType<LoginResponse>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> Token([FromBody] LoginRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(Token));

        try
        {
            var baseResponse = new BaseResponse();

            var validator = await new LoginRequestValidator(baseResponse.Warnings).ValidateAsync(request);

            if (!validator.IsValid)
            {
                validator.Errors.ForEach( e =>
                    baseResponse.Errors.Add( new ValidationError { 
                        PropertyName = e.PropertyName, 
                        ErrorMessage = e.ErrorMessage 
                    }) 
                );
                return BadRequest(baseResponse);
            }

            var response = await _tokenService.GenerateToken(request);

            baseResponse.Warnings.ForEach(response.Warnings.Add);

            _logger.LogInformation(LogMessage.EndMethod, nameof(Token));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(Token), ex.Message);
            return BadRequest( new BaseResponse { 
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ] 
            });
        }
    }
}
