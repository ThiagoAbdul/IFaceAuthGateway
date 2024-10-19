using IFaceAuthService.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace IFaceAuthService.Extensions;

public static class AuthenticationExtension
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services
        .AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = false;
            x.TokenValidationParameters = JsonWebTokenHelper.GetTokenValidationParameters(configuration);
        });

        services.AddAuthorization();
    }
}
