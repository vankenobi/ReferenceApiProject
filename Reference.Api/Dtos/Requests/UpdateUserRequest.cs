using System;
using Reference.Api.Utils;

namespace Reference.Api.Dtos.Requests
{
	public class UpdateUserRequest : ICommonProperties
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

