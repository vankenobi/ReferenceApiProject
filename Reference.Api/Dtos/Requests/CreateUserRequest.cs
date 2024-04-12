using System;

namespace Reference.Api.Dtos.Requests
{
    public class CreateUserRequest : ICommonProperties
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

