using Corely.IAM.Groups.Constants;
using Corely.IAM.Groups.Models;
using Corely.IAM.Groups.Validators;
using Corely.UnitTests.ClassData;
using FluentValidation.TestHelper;

namespace Corely.UnitTests.IAM.Groups.Validators;

public class GroupValidatorTests
{
    private readonly GroupValidator _validator = new();

    [Theory]
    [ClassData(typeof(NullEmptyAndWhitespace))]
    public void GroupValidator_HasValidationError_WhenGroupNameInvalid(string groupName)
    {
        var group = new Group
        {
            GroupName = groupName
        };

        var result = _validator.TestValidate(group);
        result.ShouldHaveValidationErrorFor(x => x.GroupName);
    }


    [Theory, MemberData(nameof(InvalidGroupTestData))]
    public void GroupValidator_HasValidationError_WhenGroupNameLengthInvalid(string groupName)
    {
        var group = new Group
        {
            GroupName = groupName
        };

        var result = _validator.TestValidate(group);
        result.ShouldHaveValidationErrorFor(x => x.GroupName);
    }

    public static IEnumerable<object[]> InvalidGroupTestData =>
    [
        [new string('a', GroupConstants.GROUP_NAME_MIN_LENGTH - 1)],
            [new string('a', GroupConstants.GROUP_NAME_MAX_LENGTH + 1)]
    ];
}
