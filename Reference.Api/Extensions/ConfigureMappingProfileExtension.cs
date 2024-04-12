using System;
using AutoMapper;
using Reference.Api.Mapper;

namespace Reference.Api.Extensions
{
	public static class ConfigureMappingProfileExtension
	{
		public static IServiceCollection ConfigureMapping(this IServiceCollection service)
		{
			var mappingConfig = new MapperConfiguration(i => i.AddProfile(new AutoMapperMappingProfile()));
			IMapper mapper = mappingConfig.CreateMapper();

			service.AddSingleton(mapper);

			return service; 
		}
	}
}

