using Microsoft.EntityFrameworkCore;
using WebApplication1.Infrastructure;

namespace WebApplication1.Extensions;

public static class DatabaseExtensions
{
    public static async Task MigrateDatabaseAsync(this IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();

        var services = serviceScope.ServiceProvider;
        var dbContext = services.GetRequiredService<AppDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}