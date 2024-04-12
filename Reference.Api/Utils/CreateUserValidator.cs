using System;
using FluentValidation;
using Reference.Api.Dtos.Requests;

namespace Reference.Api.Utils
{
	public class CreateUserValidator : UserBaseValidator<CreateUserRequest>
	{
		public CreateUserValidator()
		{
			
        }
	}


}

