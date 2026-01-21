using FluentValidation;
using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class AddCandidatesToDriveRequestValidator : AbstractValidator<AddCandidatesToDriveRequest>
{
    public AddCandidatesToDriveRequestValidator(List<object> warnings, RepoService repoService,
        IUserProvider userProvider)
    {
        RuleFor(e => e.DriveId)
            .NotEmpty();
        RuleFor(e => e.CandidateIds)
            .NotNull();
    }
}