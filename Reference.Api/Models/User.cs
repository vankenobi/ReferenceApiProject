using System;
using Reference.Api.Enums;

namespace Reference.Api.Models
{
	public class User : BaseEntity
	{
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

