using EduBot.Application.Common.Interfaces;
using EduBot.Application.Common.Services;
using EduBot.Infrastructure.Configurations;
using EduBot.Infrastructure.Identity;
using EduBot.Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System.Text;
using Vips.EstoqueBase.Application.Common.Interfaces.Persistence;
using Vips.EstoqueBase.Infrastructure.Persistence;
using Vips.EstoqueBase.Infrastructure.Persistence.MongoDb;

namespace EduBot.Infrastructure {
    public static class DependencyInjectionRegister {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>("mongodb://localhost:27017", "Identity");

            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IAuthenticate, AuthenticateService>();
            services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
            services.AddScoped<IConversationsRepository, ConversationsMongoDbRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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

        public static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"] ?? "")),
                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }

        public static IServiceCollection ConfigureDatabase(
            this IServiceCollection services,
            IConfiguration configuration
        ) {
            var mongoDbSettings = new MongoDbSettings();
            configuration.GetSection(MongoDbSettings.SectionName).Bind(mongoDbSettings);
            mongoDbSettings.ConnectionString = CreateMongoDbConnectionString(configuration);
            services.AddSingleton(Options.Create(mongoDbSettings));

            return services;
        }

        private static string CreateMongoDbConnectionString(IConfiguration configuration) {
            string mongoDbConnectionString =
                configuration["MongoDbSettings:ConnectionString"] ?? "";

            return mongoDbConnectionString;
        }
    }
}
