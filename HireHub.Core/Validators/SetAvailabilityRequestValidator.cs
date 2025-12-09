using FluentValidation;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class SetAvailabilityRequestValidator : AbstractValidator<List<string>>
{
    public SetAvailabilityRequestValidator(List<object> warnings, RepoService repoService, IUserProvider userProvider)
    {

        RuleFor(e => e).CustomAsync( async (request, context, cancellationToken) => 
        {
            if (request.Count == 0)
                warnings.Add(ResponseMessage.NoSlotsProvided);

            bool IsSlotNotExist = false, IsPastSlotExist = false;
            request.ForEach( slotId =>
            {
                var isExist = repoService.SlotRepository.ExistsAsync(int.Parse(slotId), cancellationToken);
                if (!isExist.WaitAsync(CancellationToken.None).Result)
                    { IsSlotNotExist = true; return; }

                var isPast = repoService.SlotRepository.IsPastSlot(int.Parse(slotId), cancellationToken);
                if (isPast.WaitAsync(CancellationToken.None).Result)
                    { IsPastSlotExist = true; return; }
            });
            if (IsSlotNotExist) context.AddFailure(PropertyName.Main, ResponseMessage.InvalidSlots);
            if (IsPastSlotExist) context.AddFailure(PropertyName.Main, ResponseMessage.PastSlotsInvalid);

            var userId = int.Parse(userProvider.CurrentUserId);
            var slotIds = request.Select(int.Parse).ToList();
            if ( await repoService.UserRepository.IsUserRegisterAnyOfTheSlots(userId, slotIds, cancellationToken) )
                warnings.Add(ResponseMessage.SlotsAlreadySet);
        });

    }

}
