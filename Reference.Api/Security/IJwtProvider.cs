using System;
using AutoMapper.Execution;
using Reference.Api.Models;

namespace Reference.Api.Security
{
	public interface IJwtProvider
	{
        string Generate(User user);
    }
}

