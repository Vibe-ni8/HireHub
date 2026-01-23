using FluentValidation;
using HireHub.Core.Data.Models;
using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class AddMemberToDriveRequestValidator : AbstractValidator<AddMemberToDriveRequest>
{
    public AddMemberToDriveRequestValidator(List<object> warnings, RepoService repoService,
        IUserProvider userProvider)
    {
        RuleFor(e => e.DriveId)
            .NotEmpty();
        RuleFor(e => e.MemberId)
            .NotEmpty();
        RuleFor(e => e.MemberRole)
            .NotEmpty();

        RuleFor(e => e).Custom((request, context) =>
        {
            var drive = repoService.DriveRepository.GetByIdAsync(request.DriveId)
                        .WaitAsync(CancellationToken.None).Result;
            if (drive == null)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.DriveNotFound);
                return;
            }

            if (drive.Status == DriveStatus.Completed)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.CannotAddMemberOnClosedDrive);
                return;
            }

            var user = repoService.UserRepository.GetByIdAsync(request.MemberId)
                        .WaitAsync(CancellationToken.None).Result;
            if (user == null)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.UserNotFound);
                return;
            }
            if (!user.IsActive)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.InactiveUser);
                return;
            }

            var alreadyAssigned = repoService.DriveRepository
                .IsUserAssignedInAnyActiveDriveOnDateAsync(user.UserId, drive.DriveDate)
                .WaitAsync(CancellationToken.None).Result;
            if (alreadyAssigned)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.UsersAssignedToAnotherActiveDriveOnSameDate);
                return;
            }

            var role = repoService.RoleRepository.GetByIdAsync(user.RoleId)
                        .WaitAsync(CancellationToken.None).Result;
            if (role!.RoleName.ToString().ToLower() != request.MemberRole.ToLower())
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.UserNotInSpecifiedRole);
                return;
            }
        });
    }
}