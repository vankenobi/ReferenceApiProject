using System;

namespace Reference.Api.Dtos.Requests
{
    public interface ICommonProperties
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}

