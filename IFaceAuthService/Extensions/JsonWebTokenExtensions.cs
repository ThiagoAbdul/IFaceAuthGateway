using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace IFaceAuthService.Extensions;

public static class JsonWebTokenExtensions
{
    public static string? SubjectId(this TokenValidationResult validationResult)
    {
        return validationResult.Claims
                         .FirstOrDefault(x => x.Key == JwtRegisteredClaimNames.Sub)
                         .Value.ToString();
    }

    public static string? JwtToken(this HttpRequest request)
    {
        StringValues authorizationHeader = request.Headers.Authorization;
        if (string.IsNullOrEmpty(authorizationHeader))
            return null;
        return authorizationHeader.ToString().Split(" ")[1];
    }

}