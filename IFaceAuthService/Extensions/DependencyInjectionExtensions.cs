using IFaceAuthService.Data;
using IFaceAuthService.Helpers;
using IFaceAuthService.Middlewares;
using IFaceAuthService.Services;
using IFaceAuthService.UseCases;
using Microsoft.EntityFrameworkCore;

namespace IFaceAuthService.Extensions;

public static class DependencyInjectionExtensions
{
    public static void AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => 
            options.UseNpgsql(
                Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
                ?? configuration.GetConnectionString("DefaultConnection")
            )
        );

        services.AddScoped<SignInUseCase>();
        services.AddScoped<SignUpUseCase>();
        services.AddScoped<JsonWebTokenHelper>();
        services.AddScoped<TokenService>();
        services.AddScoped<UserService>();

        services.AddTransient<RequestForwardingMiddleware>();
    }
}
