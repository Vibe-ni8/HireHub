using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using HireHub.Shared.Authentication.Interface;
using HireHub.Shared.Common;
using HireHub.Shared.Common.Exceptions;
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
            throw new CommonException(CommonRS.Auth_InvalidCredentials_Format(request.Username));
        }

        if (string.IsNullOrEmpty(user.PasswordHash))
        {
            return new() { Warnings = [ResponseMessage.PasswordSetRequire] };
        }

        if (!VerifyPassword(user, user.PasswordHash, request.Password))
        {
            _logger.LogWarning(LogMessage.InvalidPassword, user.UserId);
            throw new CommonException(CommonRS.Auth_InvalidCredentials_Format(request.Username));
        }

        if (!user.IsActive)
        {
            _logger.LogWarning(LogMessage.NotActiveUser, user.UserId);
            return new() { Warnings = [ResponseMessage.NotActiveUser] };
        }

        var role = await _roleRepository.GetByIdAsync(user.RoleId);

        var token = _jwtTokenService.GenerateToken(user.UserId.ToString(), role!.RoleName.ToString());

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