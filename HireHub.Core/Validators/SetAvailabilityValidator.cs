using FluentValidation;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class SetAvailabilityValidator : AbstractValidator<List<string>>
{
    public SetAvailabilityValidator(List<object> warnings, RepoService repoService, IUserProvider userProvider)
    {
        RuleFor(e => e).CustomAsync( (request, context, cancellationToken) => 
        {
            request.ForEach(async slotId =>
            {
                if (!await repoService.SlotRepository.ExistsAsync(slotId, cancellationToken))
                    context.AddFailure(PropertyName.SetAvailability, "");
            });

            return Task.CompletedTask;
        });
    }
}
