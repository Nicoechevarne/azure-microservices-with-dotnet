using Microsoft.EntityFrameworkCore;
using Polly;
using Wpm.Clinic.Application;
using Wpm.Clinic.DataAccess;
using Wpm.Clinic.ExternalServices;

namespace Wpm.Clinic;

public static class Bootstrapper
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ClinicDbContext>();
        context.Database.EnsureCreated();
    }

    public static void AddServices(this IServiceCollection services)
    {   
        services.AddScoped<ManagementService>();
        services.AddScoped<ClinicApplicationService>();
    }
    public static void AddManagmentDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ClinicDbContext>(options =>
        {
            options.UseInMemoryDatabase("WpmManagment");
        });
    }
}