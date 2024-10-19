using IFaceAuthService.DTOs.In;
using IFaceAuthService.DTOs.Out;
using IFaceAuthService.DTOs.Out.Errors;
using IFaceAuthService.Entities;
using IFaceAuthService.Helpers;
using IFaceAuthService.Models;
using IFaceAuthService.Services;

namespace IFaceAuthService.UseCases;

public class SignInUseCase(UserService userService, TokenService tokenService)
{
    public async Task<Result<SignInResponse>> ExecuteAsync(SignInRequest request)
    {
        User? savedUser = await userService.GetByEmailAsync(request.Email);

        var errorResult = Result.ForError<SignInResponse>(new IncorrrectUserOrPasswordErrorModel());

        if (savedUser == null)        
            return errorResult;

        if (CryptoHelper.VerifyHash(savedUser.HashPassword, request.Password))
        {

            SignInResponse response = CreateSignInResponse(savedUser);

            return Result.Success(response);
        }
        else return errorResult;

    }

    private SignInResponse CreateSignInResponse(User user)
    {
        string accessToken = tokenService.GenerateAccessToken(user);
        string refreshToken = tokenService.GenerateRefreshToken(user, accessToken);

        return new SignInResponse(user.Id, accessToken, refreshToken);
    }
}
