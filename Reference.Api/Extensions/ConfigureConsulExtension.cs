using System;
using Consul;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Winton.Extensions.Configuration.Consul;

namespace Reference.Api.Extensions
{

	public static class ConfigureConsulExtension
	{

        public static IServiceCollection AddConsul(IConfigurationManager _configuration, IServiceCollection _service, IWebHostEnvironment _env)
        {
            var address = new Uri(_configuration["Consul:Host"]);
            _service.AddSingleton<IConsulClient, ConsulClient>(consul => new ConsulClient(consulConfig =>
            {
                consulConfig.Address = address;
            }, null, handlerOverride =>
            {
                handlerOverride.Proxy = null;
                handlerOverride.UseProxy = false;
            }));

            return _service;
        }

        public static IApplicationBuilder ServiceRegistration(IConfigurationManager _configuration, IApplicationBuilder app)
        {
            
            var consulClient = app.ApplicationServices.GetService<IConsulClient>();
            var serviceConfig = _configuration.GetSection("service").Get<ConsulConfig>();
            //var logger = app.ApplicationServices.GetService<ILogger>();
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            var registration = new AgentServiceRegistration()
            {
                ID = serviceConfig.Id,
                Name = serviceConfig.Name, // Servis adı
                Address = serviceConfig.Address, // Servisin adresi
                Port = serviceConfig.Port, // Servisin portu
                Tags = serviceConfig.Tags // new[] { "tag1", "tag2" } // Servis etiketleri
            };

            consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            consulClient.Agent.ServiceRegister(registration).Wait();
            //logger.LogInformation($"Service registered with id: {serviceConfig.Id}");

            lifetime.ApplicationStopped.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();
                //logger.LogInformation($"Service deregistered with id: {serviceConfig.Id}");
            });

            return app;
        }

     
    }
}

