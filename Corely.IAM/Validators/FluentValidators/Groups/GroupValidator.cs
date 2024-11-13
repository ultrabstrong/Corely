using Corely.IAM.Groups.Constants;
using Corely.IAM.Groups.Models;
using FluentValidation;

namespace Corely.IAM.Validators.FluentValidators.Groups
{
    internal class GroupValidator : AbstractValidator<Group>
    {
        public GroupValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MinimumLength(GroupConstants.GROUP_NAME_MIN_LENGTH)
                .MaximumLength(GroupConstants.GROUP_NAME_MAX_LENGTH);
        }
    }
}
