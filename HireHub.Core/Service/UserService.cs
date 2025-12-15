using HireHub.Core.Data.Filters;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UserService> _logger;
    private readonly ISaveRepository _saveRepository;

    public UserService(IUserRepository userRepository, IRoleRepository roleRepository,
        ILogger<UserService> logger, ISaveRepository saveRepository)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logger = logger;
        _saveRepository = saveRepository;
    }


    #region Query Services

    public async Task<Response<List<UserDTO>>> GetUsers(UserRole? role, bool? isActive,
        bool isLatestFirst, DateTime? startDate, DateTime? endDate, int? pageNumber, int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetUsers));

        var filter = new UserFilter
        {
            Role = role, IsActive = isActive, IsLatestFirst = isLatestFirst,
            StartDate = startDate, EndDate = endDate,
            PageNumber = pageNumber, PageSize = pageSize
        };
        var users = await _userRepository.GetAllAsync(filter, CancellationToken.None);

        var userDTOs = ConverToDTO(users);

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetUsers));

        return new Response<List<UserDTO>>
        {
            Data = userDTOs
        };
    }

    #endregion

    #region Command Services

    public async Task<Response<UserDTO>> AddUser(AddUserRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(AddUser));

        var user = Helper.Map<AddUserRequest, User>(request);

        var role = await _roleRepository
            .GetByName((UserRole)Enum.Parse(typeof(UserRole), request.RoleName, true));
        user.RoleId = role.RoleId;

        var hasher = new PasswordHasher<User>();
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        await _userRepository.AddAsync(user, CancellationToken.None);
        _saveRepository.SaveChanges();

        var userDTO = Helper.Map<User, UserDTO>(user);
        userDTO.RoleName = role.RoleName.ToString();

        _logger.LogInformation(LogMessage.EndMethod, nameof(AddUser));

        return new() { Data = userDTO };
    }

    #endregion

    #region Private Methods

    private List<UserDTO> ConverToDTO(List<User> users)
    {
        var userDTOs = new List<UserDTO>();
        users.ForEach(user =>
        {
            var userDTO = Helper.Map<User, UserDTO>(user);
            userDTO.RoleName = user.Role!.RoleName.ToString();
            userDTOs.Add(userDTO);
        });
        return userDTOs;
    }

    #endregion

}
