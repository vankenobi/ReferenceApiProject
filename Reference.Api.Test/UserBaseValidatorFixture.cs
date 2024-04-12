using System;
using Reference.Api.Dtos.Requests;
using Reference.Api.Utils;

namespace Reference.Api.Test
{
	[TestFixture]
	public class UserBaseValidatorFixture
	{
		private readonly CreateUserValidator _createUserValidator;

		public UserBaseValidatorFixture()
		{
            _createUserValidator = new();
		}

        [Test]
        public void Validate_NameEmpthy_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_InvalidNameLengthMoreThan20_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "JohnDoeDoeDoeDoeDoeDoe",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PasswordEmpty_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_PasswordLengthMoreThan20_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "JohnDoe",
                Email = "john@example.com",
                Password = "password12345678910123456"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_EmailEmpty_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "JohnDoe",
                Email = "",
                Password = "password123"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public void Validate_InvalidEmailAddress_ReturnsFalse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "JohnDoe",
                Email = "johnhotmail.com",
                Password = "password123"
            };

            var result = _createUserValidator.Validate(createUserRequest);

            Assert.IsFalse(result.IsValid);
        }
    }
}

