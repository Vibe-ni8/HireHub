using HireHub.Core.Data.Filters;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using HireHub.Shared.Common.Exceptions;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class DriveService
{
    private readonly IDriveRepository _driveRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly ILogger<DriveService> _logger;

    public DriveService(IDriveRepository driveRepository, IRoleRepository roleRepository,
        IUserRepository userRepository,
        ISaveRepository saveRepository, ILogger<DriveService> logger)
    {
        _driveRepository = driveRepository;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _saveRepository = saveRepository;
        _logger = logger;
    }


    #region Query Services

    public async Task<Response<List<DriveDTO>>> GetDrives(DriveStatus? status,
        string? creatorEmail, int? technicalRounds, bool isLatestFirst, bool includePastDrives, 
        DateTime? startDate, DateTime? endDate, int? pageNumber, int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDrives));

        var filter = new DriveFilter
        {
            Status = status,
            CreatorEmail = creatorEmail,
            TechnicalRounds = technicalRounds,
            IsLatestFirst = isLatestFirst,
            IncludePastDrives = includePastDrives,
            StartDate = startDate,
            EndDate = endDate,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
        var drives = await _driveRepository.GetAllAsync(filter, CancellationToken.None);

        var driveDTOs = ConverToDTO(drives);

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetDrives));

        return new()
        {
            Data = driveDTOs
        };
    }


    public async Task<Response<DriveDTO>> GetDrive(int driveId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDrive));

        var drive = await _driveRepository.GetByIdAsync(driveId) ??
            throw new CommonException(ResponseMessage.DriveNotFound);

        var driveDTO = Helper.Map<Drive, DriveDTO>(drive);
        driveDTO.CreatorName = (await _userRepository.GetByIdAsync(drive.CreatedBy))!.FullName;

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetDrive));

        return new() { Data = driveDTO };
    }


    public async Task<Response<DriveConfigDTO>> GetDriveConfig(int driveId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDriveConfig));

        var driveWithConfig = await _driveRepository.GetDriveWithConfigAsync(driveId) ??
            throw new CommonException(ResponseMessage.DriveNotFound);

        var roles = await _roleRepository.GetAllAsync(CancellationToken.None);
        var hrRole = roles.First(e => e.RoleName == UserRole.HR);
        var panelRole = roles.First(e => e.RoleName == UserRole.Panel);
        var mentorRole = roles.First(e => e.RoleName == UserRole.Mentor);

        var hrConfig = driveWithConfig.DriveRoleConfigurations.First(e => e.RoleId == hrRole.RoleId);
        var panelConfig = driveWithConfig.DriveRoleConfigurations.First(e => e.RoleId == panelRole.RoleId);
        var mentorConfig = driveWithConfig.DriveRoleConfigurations.First(e => e.RoleId == mentorRole.RoleId);

        var driveConfigDTO = new DriveConfigDTO
        {
            DriveId = driveId,
            HrConfiguration = Helper.Map<DriveRoleConfiguration, HrConfigurationDTO>(hrConfig),
            PanelConfiguration = Helper.Map<DriveRoleConfiguration, PanelConfigurationDTO>(hrConfig),
            MentorConfiguration = Helper.Map<DriveRoleConfiguration, MentorConfigurationDTO>(hrConfig),
            PanelVisibilitySettings = Helper.Map<PanelVisibilitySettings, PanelVisibilitySettingsDTO>(driveWithConfig.PanelVisibilitySettings!),
            NotificationSettings = Helper.Map<NotificationSettings, NotificationSettingsDTO>(driveWithConfig.NotificationSettings!),
            FeedbackConfiguration = Helper.Map<FeedbackConfiguration, FeedbackConfigurationDTO>(driveWithConfig.FeedbackConfiguration!)
        };

        _logger.LogInformation(LogMessage.EndMethod, nameof(GetDriveConfig));

        return new() { Data = driveConfigDTO };
    }

    #endregion

    #region Command Services

    public async Task<Response<DriveDTO>> CreateDriveAsync(CreateDriveRequest request, int requestUserId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(CreateDriveAsync));

        var drive = new Drive
        {
            DriveName = request.DriveName,
            DriveDate = request.DriveDate,
            TechnicalRounds = request.TechnicalRounds,
            Status = DriveStatus.InProposal,
            CreatedBy = requestUserId,
            CreatedDate = DateTime.Now
        };

        // -------------------------------
        // Drive Members
        // -------------------------------
        var hrRole = await _roleRepository.GetByName(UserRole.HR);
        request.CoordinationTeam.Hrs.ForEach( hrId =>
            drive.DriveMembers.Add( new()
            {
                UserId = hrId,
                RoleId = hrRole.RoleId
            })
        );
        var mentorRole = await _roleRepository.GetByName(UserRole.Mentor);
        request.CoordinationTeam.Mentors.ForEach( mentorId =>
            drive.DriveMembers.Add( new()
            {
                UserId = mentorId,
                RoleId = mentorRole.RoleId
            })
        );
        var panelRole = await _roleRepository.GetByName(UserRole.Panel);
        request.CoordinationTeam.PanelMembers.ForEach(panelmemberId =>
            drive.DriveMembers.Add( new()
            {
                UserId = panelmemberId,
                RoleId = panelRole.RoleId
            })
        );

        // -------------------------------
        // Role Configurations
        // -------------------------------
        var hrRoleConfig = Helper.Map<HrConfigRequest, DriveRoleConfiguration>(request.HrConfiguration);
        hrRoleConfig.RoleId = hrRole.RoleId;
        hrRoleConfig.CanViewFeedback = true;
        drive.DriveRoleConfigurations.Add(hrRoleConfig);

        var panelRoleConfig = Helper.Map<PanelConfigRequest, DriveRoleConfiguration>(request.PanelConfiguration);
        panelRoleConfig.RoleId = panelRole.RoleId;
        panelRoleConfig.AllowBulkUpload = false;
        panelRoleConfig.CanViewFeedback = true;
        drive.DriveRoleConfigurations.Add(panelRoleConfig);

        var mentorRoleConfig = Helper.Map<MentorConfigRequest, DriveRoleConfiguration>(request.MentorConfiguration);
        mentorRoleConfig.RoleId = mentorRole.RoleId;
        mentorRoleConfig.AllowBulkUpload = false;
        mentorRoleConfig.CanEditSubmittedFeedback = false;
        drive.DriveRoleConfigurations.Add(mentorRoleConfig);

        // -------------------------------
        // Panel Visibility
        // -------------------------------
        drive.PanelVisibilitySettings = Helper
            .Map<PanelVisibilityConfigRequest, PanelVisibilitySettings>(request.PanelVisibilityConfiguration);

        // -------------------------------
        // Notification
        // -------------------------------
        drive.NotificationSettings = Helper
            .Map<NotificationConfigRequest, NotificationSettings>(request.NotificationConfiguration);

        // -------------------------------
        // Feedback Configuration
        // -------------------------------
        drive.FeedbackConfiguration = Helper
            .Map<FeedbackConfigRequest, FeedbackConfiguration>(request.FeedbackSettings);

        await _driveRepository.AddAsync(drive, CancellationToken.None);
        _saveRepository.SaveChanges();

        _logger.LogInformation(LogMessage.EndMethod, nameof(CreateDriveAsync));

        return new() { Data = Helper.Map<Drive, DriveDTO>(drive) };
    }

    #endregion

    #region Private Methods

    private List<DriveDTO> ConverToDTO(List<Drive> drives)
    {
        var driveDTOs = new List<DriveDTO>();
        drives.ForEach(drive =>
        {
            var driveDTO = Helper.Map<Drive, DriveDTO>(drive);
            driveDTO.CreatorName = drive.Creator!.FullName;
            driveDTO.DriveStatus = drive.Status.ToString();
            driveDTOs.Add(driveDTO);
        });
        return driveDTOs;
    }

    #endregion
}
