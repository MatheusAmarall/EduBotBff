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
        .AddInfrastructure(builder.Configuration);

string? origin = builder.Configuration["OriginUrl"];

if (!string.IsNullOrEmpty(origin)) {
    builder.Services.AddCors(o =>
        o.AddPolicy(
            "CorsPolicy",
            corsPolicyBuilder => {
                corsPolicyBuilder.WithOrigins(origin).AllowAnyMethod().AllowAnyHeader();
            }
        )
    );
}
else {
    builder.Services.AddCors();
}

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!string.IsNullOrEmpty(origin)) {
    app.UseCors("CorsPolicy");
}

SeedUserRoles(app);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void SeedUserRoles(IApplicationBuilder app) {
    using (var serviceScope = app.ApplicationServices.CreateScope()) {
        var seed = serviceScope.ServiceProvider.GetService<ISeedUserRoleInitial>();

        seed.SeedRoles();
        seed.SeedUsers();
    }
}
