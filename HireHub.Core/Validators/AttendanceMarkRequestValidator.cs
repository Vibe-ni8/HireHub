using FluentValidation;
using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class AttendanceMarkRequestValidator : AbstractValidator<AttendanceMarkRequest>
{
    public AttendanceMarkRequestValidator(List<object> warnings, RepoService repoService,
        IUserProvider userProvider)
    {
        RuleFor(e => e.CandidateId).NotEmpty();
        RuleFor(e => e.UserSlotId).NotEmpty();

        RuleFor(e => e).CustomAsync(async (request, context, cancellationToken) =>
        {
            var candidate = await repoService.CandidateRepository
                .GetByIdAsync(request.CandidateId, cancellationToken);
            if (candidate == null)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.InvalidCandidate);
                return;
            }

            var userSlot = await repoService.UserSlotRepository
                .GetByIdAsync(request.UserSlotId, cancellationToken);
            if (userSlot == null)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.InvalidPanel);
                return;
            }

            var candidateMap = await repoService.CandidateMapRepository
                .GetByIdAsync(request.CandidateId, request.UserSlotId, cancellationToken);
            if (candidateMap == null)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.CandidateNotAssignedToPanel);
                return;
            }

            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);
            var currentTime = TimeOnly.FromDateTime(now);

            var slot = await repoService.SlotRepository
                .GetByIdAsync(userSlot.SlotId, cancellationToken);
            if ((candidateMap.IsPresent != null) && (slot!.SlotDate < today || (slot.SlotDate == today && slot.EndTime < currentTime)))
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.CanNotChangeOrSetAttendanceForPastCandidate);
                return;
            }
        });
    }
}
