using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Utils.Common;
using HireHub.Shared.Authentication.Interface;
using HireHub.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class TokenService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly ILogger<TokenService> _logger;

    public TokenService(IUserRepository userRepository, IRoleRepository roleRepository, 
        IJwtTokenService jwtTokenService,ILogger<TokenService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
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
            return new() {
                Errors = [
                    new ValidationError { 
                        PropertyName = PropertyName.Main, 
                        ErrorMessage = CommonRS.Auth_InvalidCredentials_Format(request.Username) 
                    }
                ] 
            };
        }

        if (!user.IsActive)
        {
            _logger.LogWarning(LogMessage.NotActiveUser, user.UserId);
            return new()
            {
                Errors = [
                    new ValidationError {
                        PropertyName = PropertyName.Main,
                        ErrorMessage = ResponseMessage.NotActiveUser
                    }
                ]
            };
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return new() { Warnings = [ResponseMessage.PasswordSetRequire] };
        }

        if (!VerifyPassword(user, user.PasswordHash, request.Password))
        {
            _logger.LogWarning(LogMessage.InvalidPassword, user.UserId);
            return new() { 
                Errors = [
                    new ValidationError { 
                        PropertyName = PropertyName.Main, 
                        ErrorMessage = CommonRS.Auth_InvalidCredentials_Format(request.Username) 
                    }
                ] 
            };
        }

        var role = await _roleRepository.GetByIdAsync(user.RoleId);

        var token = _jwtTokenService.GenerateToken(user.UserId.ToString(), role!.RoleName);

        _logger.LogInformation(LogMessage.EndMethod, nameof(GenerateToken));

        if (request.Password.Equals("Welcome@123"))
            return new() {
                Data = token,
                Warnings = [ResponseMessage.PasswordReSetRequire] 
            };

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