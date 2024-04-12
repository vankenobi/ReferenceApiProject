using System;
using FluentValidation;
using Reference.Api.Dtos.Requests;

namespace Reference.Api.Utils
{
	public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
	{
		public UpdateUserValidator()
		{

            RuleFor(x => x.Id)
                .NotEmpty()
                .Custom((id, context) =>
                {
                    if (!Guid.TryParse(id.ToString(), out _))
                    {
                        context.AddFailure("Invalid guid format.");
                    }
                });

        }
    }
}

