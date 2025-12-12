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
    private readonly ISaveRepository _saveRepository;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger,
        ISaveRepository saveRepository)
    {
        _userRepository = userRepository;
        _logger = logger;
        _saveRepository = saveRepository;
    }

    public async Task<UserResponse<UserDetailsDTO>> GetUserDetails(int userId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetUserDetails));

        var response = new UserResponse<UserDetailsDTO>();

        var user = await _userRepository.GetByIdAsync(userId);

        if (user == null)
        {
            response.Errors.Add(new ValidationError {
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
            await SetUserSlots(response.Data.UserSlots, user.UserId);
        }
        else
        {
            response.Errors.Add(new ValidationError {
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
        userSlots.ForEach(userSlot =>
        {
            var userSlotDetail = Helper.Map<UserSlot, UserSlotDetailsDTO>(userSlot);
            var candidateMaps = _userRepository.GetCandidatesAssignedForUserOnSlot(userSlot.Id);
            candidateMaps.ForEach(candidateMap =>
            {
                var candidateDetails = Helper.Map<Candidate, CandidateDetailsDTO>(candidateMap.Candidate);
                candidateDetails.ScheduledTime = candidateMap.ScheduledTime;
                candidateDetails.IsPresent = candidateMap.IsPresent;
                candidateDetails.InterviewRounds = candidateMap.InterviewRounds;
                candidateDetails.FeedbackId = candidateMap.FeedbackId;
                candidateDetails.Feedback = candidateMap.Feedback != null ?
                    Helper.Map<Feedback, FeedbackDTO>(candidateMap.Feedback) : null;
                candidateDetails.IsSelected = candidateMap.IsSelected;

                userSlotDetail.Candidates.Add(candidateDetails);
            });
            userSlotDetails.Add(userSlotDetail);
        });
    }

}
