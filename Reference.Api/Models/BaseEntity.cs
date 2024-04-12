using System;
namespace Reference.Api.Models
{
	public class BaseEntity
	{
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; } 
    }
}

