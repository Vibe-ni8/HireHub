using FluentValidation;
using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class SetFeedbackRequestValidator : AbstractValidator<SetFeedbackRequest>
{
    public SetFeedbackRequestValidator(List<object> warnings, RepoService repoService, 
        IUserProvider userProvider)
    {
        RuleFor(e => e.CandidateId).NotEmpty();
        RuleFor(e => e.UserSlotId).NotEmpty();
        RuleFor(e => e.StarRating).LessThanOrEqualTo(5);
        RuleFor(e => e.TechnicalSkills).NotEmpty();
        RuleFor(e => e.CommunicationSkill).NotEmpty();
        RuleFor(e => e.ProblemSolvingAbility).NotEmpty();
        RuleFor(e => e.OverallFeedback).NotEmpty();
        RuleFor(e => e.Recommendation).NotEmpty().Must(e => Options.Recommendations.Contains(e));

        RuleFor(e => e).CustomAsync( async (request, context, cancellationToken) =>
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

            if (userSlot.UserId != int.Parse(userProvider.CurrentUserId))
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.UserDifferFromLoggedUser);
                return;
            }

            var now = DateTime.Now;
            var today = DateOnly.FromDateTime(now);

            var slot = await repoService.SlotRepository
                .GetByIdAsync(userSlot.SlotId, cancellationToken);
            if (slot!.SlotDate < today)
            {
                context.AddFailure(PropertyName.Main, ResponseMessage.CanNotSetFeedbackForPastCandidate);
                return;
            }
        });
    }
}
