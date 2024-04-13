using System;
using Reference.Api.Dtos.Requests;
using Reference.Api.Utils;

namespace Reference.Api.Test
{
	public class UpdateUserValidatorFixture
	{
		private readonly UpdateUserValidator _updateUserValidator;

		public UpdateUserValidatorFixture()
		{
			_updateUserValidator = new();
		}

		[Test]
		public void Validate_WhenIdEmpty_ReturnsFalse()
		{
            var updateUserRequest = new UpdateUserRequest
            {
                Id = new Guid(),
                Name = "John",
                Surname = "Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = _updateUserValidator.Validate(updateUserRequest);

            Assert.That(result.IsValid, Is.False);  
        }

        [Test]
        public void Validate_WhenGuidPatternValid_ReturnsTrue()
        {
            var updateUserRequest = new UpdateUserRequest
            {
                Id = new Guid("bd957bf1-aa3b-4650-986d-856aade244f1"),
                Name = "John",
                Surname = "Doe",
                Email = "john@example.com",
                Password = "password123"
            };

            var result = _updateUserValidator.Validate(updateUserRequest);

            Assert.That(result.IsValid, Is.True);
        }
	}
}

