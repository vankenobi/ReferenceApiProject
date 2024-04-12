using System;
using FluentValidation;
using Reference.Api.Dtos.Requests;

namespace Reference.Api.Utils
{
	public abstract class UserBaseValidator<T> : AbstractValidator<T> where T: ICommonProperties
    {
		public UserBaseValidator()
		{
            RuleFor(x => x)
           .NotNull();

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
	}
}

