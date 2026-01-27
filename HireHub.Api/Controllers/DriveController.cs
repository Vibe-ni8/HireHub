using HireHub.Api.Utils.Filters;
using HireHub.Core.Data.Interface;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.DTO.Base;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;
using HireHub.Core.Validators;
using HireHub.Shared.Authentication.Filters;
using HireHub.Shared.Common.Exceptions;
using HireHub.Shared.Common.Models;
using HireHub.Shared.Middleware.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace HireHub.Api.Controllers;

[RequireAuth]
[Route("api/[controller]")]
[ApiController]
public class DriveController : ControllerBase
{
    private readonly DriveService _driveService;
    private readonly IUserProvider _userProvider;
    private readonly RepoService _repoService;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<DriveController> _logger;

    public DriveController(DriveService driveService, IUserProvider userProvider,
        RepoService repoService, ITransactionRepository transactionRepository,
        ILogger<DriveController> logger)
    {
        _driveService = driveService;
        _userProvider = userProvider;
        _repoService = repoService;
        _transactionRepository = transactionRepository;
        _logger = logger;
    }


    #region Get API's

    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.View)]
    [HttpGet("fetch/all")]
    [ProducesResponseType<Response<List<DriveDTO>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetDrives([FromQuery] string? driveStatus,
        [FromQuery] string? creatorEmail, [FromQuery] int? technicalRounds, [FromQuery] bool isLatestFirst, 
        [FromQuery] bool includePastDrives, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate,
        [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDrives));

        try
        {
            object? status = null;
            if (driveStatus != null && !Enum.TryParse(typeof(DriveStatus), driveStatus, true, out status))
                throw new CommonException(ResponseMessage.InvalidDriveStatus);

            var response = await _driveService.GetDrives(
                status != null ? (DriveStatus)status : null,
                creatorEmail, technicalRounds, isLatestFirst, includePastDrives,
                startDate, endDate, pageNumber, pageSize);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetDrives));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetDrives), ex.Message);
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [HttpGet("fetch/{driveId:int}")]
    [ProducesResponseType<Response<DriveDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetDrive([FromRoute] int driveId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDrive));

        try
        {
            var response = await _driveService.GetDrive(driveId);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetDrive));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetDrive), ex.Message);
            return BadRequest(new BaseResponse()
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [HttpGet("config/fetch/{driveId:int}")]
    [ProducesResponseType<Response<DriveConfigDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetDriveConfig([FromRoute] int driveId)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDriveConfig));

        try
        {
            var response = await _driveService.GetDriveConfig(driveId);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetDriveConfig));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetDriveConfig), ex.Message);
            return BadRequest(new BaseResponse()
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [HttpGet("members/fetch/all")]
    [ProducesResponseType<Response<List<DriveMemberDTO>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> GetDriveMembers([FromQuery] int? driveId, [FromQuery] int? userId, [FromQuery] string? role,
        [FromQuery] string? driveStatus, [FromQuery] bool isLatestFirst, [FromQuery] bool includePastDrives, 
        [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(GetDriveMembers));

        try
        {
            object? userRole = null;
            if (role != null && !Enum.TryParse(typeof(UserRole), role, true, out userRole))
                throw new CommonException(ResponseMessage.InvalidRole);
            object? status = null;
            if (driveStatus != null && !Enum.TryParse(typeof(DriveStatus), driveStatus, true, out status))
                throw new CommonException(ResponseMessage.InvalidDriveStatus);

            var response = await _driveService.GetDriveMembers(driveId, userId, 
                userRole != null ? (UserRole)userRole : null, status != null ? (DriveStatus)status : null, 
                isLatestFirst, includePastDrives, startDate, endDate, pageNumber, pageSize);

            _logger.LogInformation(LogMessage.EndMethod, nameof(GetDriveMembers));

            return Ok(response);
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(GetDriveMembers), ex.Message);
            return BadRequest(new BaseResponse()
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion

    #region Post API's

    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Add)]
    [HttpPost("create")]
    [ProducesResponseType<Response<DriveDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> CreateDrive([FromBody] CreateDriveRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(CreateDrive));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new CreateDriveRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var currentUserId = int.Parse(_userProvider.CurrentUserId);
                var response = await _driveService.CreateDriveAsync(request, currentUserId);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(CreateDrive));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(CreateDrive), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpPost("candidates/add")]
    [ProducesResponseType<Response<List<DriveCandidateDTO>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> AddCandidatesToDrive([FromBody] AddCandidatesToDriveRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(AddCandidatesToDrive));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new AddCandidatesToDriveRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.AddCandidatesToDriveAsync(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(AddCandidatesToDrive));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(AddCandidatesToDrive), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpPost("member/add")]
    [ProducesResponseType<Response<DriveMemberDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> AddMemberToDrive([FromBody] AddMemberToDriveRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(AddMemberToDrive));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new AddMemberToDriveRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.AddMemberToDriveAsync(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(AddMemberToDrive));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(AddMemberToDrive), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion

    #region Put API's

    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpPut("edit")]
    [ProducesResponseType<Response<DriveDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> EditDrive([FromBody] JObject request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(EditDrive));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new
                    EditDriveRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.EditDrive(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(EditDrive));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(EditDrive), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpPut("config/edit")]
    [ProducesResponseType<Response<DriveConfigDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> EditDriveConfig([FromBody] JObject request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(EditDriveConfig));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new
                    EditDriveConfigRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.EditDriveConfig(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(EditDriveConfig));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(EditDriveConfig), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion

    #region Delete API's

    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpDelete("member/remove")]
    [ProducesResponseType<Response<DriveMemberDTO>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> RemoveDriveMember([FromBody] RemoveDriveMemberRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(RemoveDriveMember));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new
                    RemoveDriveMemberRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.RemoveDriveMember(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(RemoveDriveMember));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(RemoveDriveMember), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }


    [RequireAuth([RoleName.Admin])]
    [RequirePermission(UserAction.Drive, ActionType.Update)]
    [HttpDelete("candidates/remove")]
    [ProducesResponseType<Response<List<int>>>(200)]
    [ProducesResponseType<BaseResponse>(400)]
    [ProducesResponseType<ErrorResponse>(500)]
    public async Task<IActionResult> RemoveDriveCandidates([FromBody] RemoveDriveCandidatesRequest request)
    {
        _logger.LogInformation(LogMessage.StartMethod, nameof(RemoveDriveCandidates));

        try
        {
            using (_transactionRepository.BeginTransaction())
            {
                var baseResponse = new BaseResponse();

                var validator = await new
                    RemoveDriveCandidatesRequestValidator(baseResponse.Warnings, _repoService, _userProvider)
                    .ValidateAsync(request);

                if (!validator.IsValid)
                {
                    validator.Errors.ForEach(e =>
                        baseResponse.Errors.Add(new ValidationError
                        {
                            PropertyName = e.PropertyName,
                            ErrorMessage = e.ErrorMessage
                        })
                    );
                    return BadRequest(baseResponse);
                }

                var response = await _driveService.RemoveCandidatesFromDrive(request);

                baseResponse.Warnings.ForEach(response.Warnings.Add);

                _transactionRepository.CommitTransaction();

                _logger.LogInformation(LogMessage.EndMethod, nameof(RemoveDriveCandidates));

                return Ok(response);
            }
        }
        catch (CommonException ex)
        {
            _logger.LogWarning(LogMessage.EndMethodException, nameof(RemoveDriveCandidates), ex.Message);
            _transactionRepository.RollbackTransaction();
            return BadRequest(new BaseResponse
            {
                Errors = [
                    new ValidationError { PropertyName = PropertyName.Main, ErrorMessage = ex.Message }
                ]
            });
        }
    }

    #endregion
}