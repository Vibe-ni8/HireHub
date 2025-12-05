using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using HireHub.Shared.Authentication.Interface;
using HireHub.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class TokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IUserRepository userRepository, 
        IJwtTokenService jwtTokenService,ILogger<TokenService> logger)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _logger = logger;
    }

    public async Task<LoginResponse> GenerateToken(LoginRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GenerateToken));

        var user = await _userRepository.GetByEmailAsync(request.Username);

        if (user == null)
        {
            _logger.LogWarning(LogMessage.UserNotFoundOnLogin, request.Username);
            return new() { Errors = [new { PropertyName = PropertyName.Main, ErrorMessage = CommonRS.Auth_InvalidCredentials_Format(request.Username) }] };
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return new() { Warnings = ["Password not set"] };
        }

        if (!VerifyPassword(user, user.PasswordHash, request.Password))
        {
            _logger.LogWarning(LogMessage.InvalidPassword, user.Id);
            return new() { Errors = [new { PropertyName = PropertyName.Main, ErrorMessage = CommonRS.Auth_InvalidCredentials_Format(request.Username) }] };
        }

        var token = _jwtTokenService.GenerateToken(user.Id.ToString(), user.Role);

        _logger.LogInformation(LogMessage.EndMethod, nameof(GenerateToken));

        return new() { Data = token };
    }

    private bool VerifyPassword(User user, string storedPasswordHash, string providedPassword)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(VerifyPassword));

        var hasher = new PasswordHasher<User>();
        // To verify:
        var result = hasher.VerifyHashedPassword(user, storedPasswordHash, providedPassword);

        _logger.LogInformation(LogMessage.EndMethod, nameof(VerifyPassword));

        return result == PasswordVerificationResult.Success;
    }
}