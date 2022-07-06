using FluentValidation;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Validations
{
    /// <summary>
    /// Represents a validator class for user
    /// </summary>
    public class TokenUserValidator : AbstractValidator<TokenUser>
    {
        /// <summary>
        /// Creates a new instance of token validator
        /// </summary>
        public TokenUserValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId.Required");
        }
    }
}

