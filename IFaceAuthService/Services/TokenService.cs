using IFaceAuthService.Entities;
using IFaceAuthService.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IFaceAuthService.Services;

public class TokenService(JsonWebTokenHelper jwtHelper)
{
    public string GenerateAccessToken(User user)
    {
        IEnumerable<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Name, user.FullName)
            ];
        return jwtHelper.Create(claims, 1);
    }

    public string GenerateRefreshToken(User user, string accesssToken)
    {
        IEnumerable<Claim> claims =
            [
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new("access", accesssToken)
            ];
        return jwtHelper.Create(claims, 24);
    }
}
