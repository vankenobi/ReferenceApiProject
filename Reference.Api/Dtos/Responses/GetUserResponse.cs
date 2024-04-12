using System;
using Reference.Api.Models;

namespace Reference.Api.Dtos.Responses
{
	public class GetUserResponse 
	{
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

