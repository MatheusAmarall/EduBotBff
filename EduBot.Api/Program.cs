using EduBot.Application;
using EduBot.Application.Common.Interfaces;
using EduBot.Infrastructure;
using EduBot.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvironmentVariables.RASA_URL))) {
    builder.Configuration["ServicesUrls:Rasa"] = Environment.GetEnvironmentVariable(
        EnvironmentVariables.RASA_URL
    );
}

builder.Services
        .AddApplication()
        .AddInfrastructure(builder.Configuration)
        .ConfigureJWT(builder.Configuration)
        .ConfigureDatabase(builder.Configuration);

builder.Services.AddCors(o =>
    o.AddPolicy(
        "CorsPolicy",
        corsPolicyBuilder => {
            corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

await SeedUserRolesAsync(app);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedUserRolesAsync(IApplicationBuilder app) {
    using (var serviceScope = app.ApplicationServices.CreateScope()) {
        var seed = serviceScope.ServiceProvider.GetService<ISeedUserRoleInitial>();

        await seed.SeedRoles();
    }
}

