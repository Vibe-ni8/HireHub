using FluentValidation;
using HireHub.Core.DTO;
using HireHub.Core.Service;
using HireHub.Core.Utils.Common;
using HireHub.Core.Utils.UserProgram.Interface;

namespace HireHub.Core.Validators;

public class AddCandidateRequestValidator : AbstractValidator<AddCandidateRequest>
{
    public AddCandidateRequestValidator(List<object> warnings, RepoService repoService,
        IUserProvider userProvider)
    {
        RuleFor(e => e.FullName).NotEmpty();
        RuleFor(e => e.Email).NotEmpty();
        RuleFor(e => e.Phone).NotEmpty();
        RuleFor(e => e.ExperienceLevelName)
            .NotEmpty()
            .Must(e => Options.ExperienceLevels.Contains(e)).WithMessage(ResponseMessage.InvalidExperienceLevel);

        RuleFor(e => e).Custom( (request, context) =>
        {
            var isEmailOrPhoneExist = repoService.CandidateRepository
            .IsCandidateWithEmailOrPhoneExist(request.Email, request.Phone)
            .WaitAsync(CancellationToken.None).Result;

            if (isEmailOrPhoneExist)
                context.AddFailure(PropertyName.Main, ResponseMessage.EmailOrPhoneAlreadyExist);
        });
    }
}
