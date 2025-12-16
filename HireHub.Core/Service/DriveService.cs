using HireHub.Core.Data.Filters;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Utils.Common;
using Microsoft.Extensions.Logging;

namespace HireHub.Core.Service;

public class DriveService
{
    private readonly IDriveRepository _driveRepository;
    private readonly ISaveRepository _saveRepository;
    private readonly ILogger<DriveService> _logger;

    public DriveService(IDriveRepository driveRepository,
        ISaveRepository saveRepository, ILogger<DriveService> logger)
    {
        _driveRepository = driveRepository;
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

    #endregion

    #region Command Services



    #endregion

    #region Private Methods

    private List<DriveDTO> ConverToDTO(List<Drive> drives)
    {
        var driveDTOs = new List<DriveDTO>();
        drives.ForEach(drive =>
        {
            var driveDTO = Helper.Map<Drive, DriveDTO>(drive);
            driveDTO.CreatorName = drive.Creator!.FullName;
            driveDTO.StatusName = drive.Status.ToString();
            driveDTOs.Add(driveDTO);
        });
        return driveDTOs;
    }

    #endregion
}
