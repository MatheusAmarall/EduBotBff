using EduBot.Application.Common.Interfaces;
using EduBot.Application.Common.Services;
using EduBot.Infrastructure.Configurations;
using EduBot.Infrastructure.Context;
using EduBot.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace EduBot.Infrastructure {
    public static class DependencyInjectionRegister {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
            string mySqlConnection = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnection,
                ServerVersion.AutoDetect(mySqlConnection), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthenticate, AuthenticateService>();
            services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();

            var servicesUrls = new ServicesUrls();
            configuration.GetSection(ServicesUrls.SectionName).Bind(servicesUrls);
            services.AddSingleton(Options.Create(servicesUrls));

            services
                .AddRefitClient<IRasaService>()
                .ConfigureHttpClient(c => {
                    c.BaseAddress = new Uri(
                        servicesUrls.Rasa
                            ?? throw new Exception(
                                "Url não configurada para o serviço do Rasa"
                            )
                    );
                });

            return services;
        }
    }
}
