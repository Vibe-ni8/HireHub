using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserProvider _userProvider;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository,
        IUserProvider userProvider, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _userProvider = userProvider;
        _logger = logger;
    }

    public async Task<UserResponse<UserDetailsDTO>> GetCurrentUserDetails()
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCurrentUserDetails));

        var response = new UserResponse<UserDetailsDTO>();

        var user = await _userProvider.CurrentUser;

        if (user == null)
        {
            response.Errors.Add( new ValidationError { 
                PropertyName = PropertyName.Main, 
                ErrorMessage = ResponseMessage.UserNotFound 
            });
        }

        response.Data = (user != null) ? Helper.Map<User, UserDetailsDTO>(user) : null;

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserDetails));

        return response;
    }

    public async Task<UserResponse<UserCompleteDetailsDTO>> GetCurrentUserCompleteDetails()
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetCurrentUserCompleteDetails));

        var response = new UserResponse<UserCompleteDetailsDTO>();
        response.Data = new UserCompleteDetailsDTO();

        var user = await _userProvider.CurrentUser;

        if (user != null)
        {
            response.Data.User = Helper.Map<User, UserDetailsDTO>(user);
            await SetUserSlots(response.Data.UserSlots, user.Id);
        }
        else
        {
            response.Errors.Add( new ValidationError { 
                PropertyName = PropertyName.Main, 
                ErrorMessage = ResponseMessage.UserNotFound 
            });
        }

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetCurrentUserCompleteDetails));

        return response;
    }

    private async Task SetUserSlots(List<UserSlotDetailsDTO> userSlotDetails, int userId)
    {
        var userSlots = await _userRepository.GetUserSlotsWithSlotDetailsForUser(userId);
        userSlots.ForEach( userSlot =>
        {
            var userSlotDetail = Helper.Map<UserSlot, UserSlotDetailsDTO>(userSlot);
            var candidateMaps = _userRepository.GetCandidatesAssignedForUserOnSlot(userSlot.Id);
            candidateMaps.ForEach(candidateMap =>
            {
                var candidateDetails = Helper.Map<Candidate, CandidateDetailsDTO>(candidateMap.Candidate);
                candidateDetails.ScheduledTime = candidateMap.ScheduledTime;
                userSlotDetail.Candidates.Add(candidateDetails);
            });
            userSlotDetails.Add(userSlotDetail);
        });
    }
}
