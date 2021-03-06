using FluentValidation;

namespace Crizzl.Infrastructure.Helpers
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .MinimumLength(6).WithMessage("Password must be at least 6 characters")
                .Matches("[A-Z]").WithMessage("Password must have 1 uppercase character")
                .Matches("[a-z]").WithMessage("Password must have 1 lowercase character")
                .Matches("[0-9]").WithMessage("Password must contain a number")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain one non alpha-numeric character");

            return options;
        }
    }
}