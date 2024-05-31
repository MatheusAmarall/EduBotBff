using EduBot.Application.Common.Interfaces;
using EduBot.Application.Common.Services;
using MediatR.Registration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace EduBot.Application {
    public static class DependencyInjectionRegister {
        public static IServiceCollection AddApplication(this IServiceCollection services) {
            services.AddScoped<IMessageService, MessageService>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
            );

            var serviceConfig = new MediatRServiceConfiguration();
            ServiceRegistrar.AddRequiredServices(services, serviceConfig);

            return services;
        }
    }
}