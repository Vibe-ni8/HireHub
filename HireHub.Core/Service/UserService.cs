using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Utils.Common;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserResponse<UserDetailsDTO>> GetUserDetails(int userId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetUserDetails));

        var response = new UserResponse<UserDetailsDTO>();

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            response.Errors.Add( new ValidationError { 
                PropertyName = PropertyName.Main, 
                ErrorMessage = ResponseMessage.UserNotFound 
            });
        }

        response.Data = (user != null) ? Helper.Map<User, UserDetailsDTO>(user) : null;

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetUserDetails));

        return response;
    }

    public async Task<UserResponse<UserCompleteDetailsDTO>> GetUserCompleteDetails(int userId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetUserCompleteDetails));

        var response = new UserResponse<UserCompleteDetailsDTO>();
        response.Data = new UserCompleteDetailsDTO();

        var user = await _userRepository.GetByIdAsync(userId);

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

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetUserCompleteDetails));

        return response;
    }

    private async Task SetUserSlots(List<UserSlotDetailsDTO> userSlotDetails, int userId)
    {
        var userSlots = await _userRepository.GetUserSlotsWithSlotDetailsForUser(userId);
        userSlots.ForEach( userSlot =>
        {
            var userSlotDetail = Helper.Map<UserSlot, UserSlotDetailsDTO>(userSlot);
            var candidateMaps = _userRepository.GetCandidatesAssignedForUserOnSlot(userSlot.Id);
            candidateMaps.ForEach( candidateMap =>
            {
                var candidateDetails = Helper.Map<Candidate, CandidateDetailsDTO>(candidateMap.Candidate);
                candidateDetails.ScheduledTime = candidateMap.ScheduledTime;
                candidateDetails.IsPresent = candidateMap.IsPresent;
                candidateDetails.InterviewRounds = candidateMap.InterviewRounds;
                candidateDetails.FeedbackId = candidateMap.FeedbackId;
                candidateDetails.Feedback = candidateMap.Feedback != null ? 
                    Helper.Map<Feedback, FeedbackDTO>(candidateMap.Feedback) : null;
                userSlotDetail.Candidates.Add(candidateDetails);
            });
            userSlotDetails.Add(userSlotDetail);
        });
    }

    public async Task<UserResponse<List<UserSlotDetailsDTO>>> SetAvailability(int userId, List<int> slotIds)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(SetAvailability));

        var response = new UserResponse<List<UserSlotDetailsDTO>>();

        var finalSlotIds = new List<int>();
        slotIds.ForEach( slotId => 
        {
            var isRegistered = _userRepository.IsUserRegisterForTheSlot(userId, slotId);
            if (!isRegistered.WaitAsync(CancellationToken.None).Result)
                finalSlotIds.Add(slotId);
        });

        if (finalSlotIds.Count == 0)
        {
            response.Data = [];
            return response;
        }

        var userSlotIds = await _userRepository.RegisterUserToSlots(userId, finalSlotIds);

        var userSlotDetails = new List<UserSlotDetailsDTO>();
        await SetUserSlots(userSlotDetails, userId);

        response.Data = userSlotDetails.Where(e => userSlotIds.Contains(e.Id)).ToList();

        _logger.LogInformation(LogMessage.EndMethod, nameof(SetAvailability));

        return response;
    }
}
