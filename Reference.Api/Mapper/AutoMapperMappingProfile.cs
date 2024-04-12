using System;
using AutoMapper;
using Reference.Api.Dtos.Requests;
using Reference.Api.Dtos.Responses;
using Reference.Api.Models;

namespace Reference.Api.Mapper
{
	public class AutoMapperMappingProfile : Profile
	{
		public AutoMapperMappingProfile()
		{
			CreateMap<User, GetUserResponse>()
				.ForMember(i => i.FullName, y => y.MapFrom(z => z.Name + " " + z.Surname))
				.ForMember(i => i.CreatedAt, y => y.MapFrom(z => z.CreatedAt.ToLocalTime()))
				.ForMember(i => i.UpdatedAt, y => y.MapFrom(z => z.UpdatedAt.ToLocalTime()));

            CreateMap<CreateUserRequest, User>();

			CreateMap<UpdateUserRequest, User>()
				.ForMember(x => x.CreatedAt, y => y.Ignore());


        }
    }
}

