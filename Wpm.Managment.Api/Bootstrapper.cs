using Microsoft.EntityFrameworkCore;
using Wpm.Managment.Api.DataAccess;

namespace Wpm.Managment.Api;

public static class Bootstrapper
{
    public static void EnsureDbIsCreated(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        var context = serviceScope.ServiceProvider.GetRequiredService<ManagmentDbContext>();
        context.Database.EnsureCreated();
    }

    public static void AddManagmentDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ManagmentDbContext>(options =>
        {
            options.UseInMemoryDatabase("WpmManagment");
        });
    }
}