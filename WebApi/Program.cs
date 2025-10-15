using Infrastructure.Data;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using WebApi.ExtensionMethods.AuthConfiguration;
using WebApi.ExtensionMethods.RegisterService;
using WebApi.ExtensionMethods.SwaggerConfiguration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// connection to database && dependency injection
builder.Services.AddRegisterService(builder.Configuration);
builder.Services.AddCors();

// register swagger configuration
builder.Services.SwaggerService();

// authentications service
builder.Services.AddAuthConfigureService(builder.Configuration);

var app = builder.Build();

app.UseCors(
    corsPolicyBuilder =>
        corsPolicyBuilder
        .WithOrigins(
            "http://127.0.0.1:5500",
            "http://localhost:3000",
            "https://localhost:3000")
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
);

try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<ApplicationContext>();
    await dataContext.Database.MigrateAsync();
    
    // seed data
    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.SeedRole();
    await seeder.SeedAdmin();
    await seeder.SeedSuperAdmin();
    await seeder.SeedCatalog();
}
catch (Exception ex)
{
    app.Logger.LogError(ex, "application reading migrate");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();