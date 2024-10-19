using IFaceAuthService.Data;
using IFaceAuthService.DTOs.In;
using IFaceAuthService.DTOs.Out;
using IFaceAuthService.DTOs.Out.Errors;
using IFaceAuthService.Entities;
using IFaceAuthService.Helpers;
using IFaceAuthService.Models;
using IFaceAuthService.Services;
using Microsoft.EntityFrameworkCore;

namespace IFaceAuthService.UseCases;

public sealed class SignUpUseCase(UserService userService)
{
    public async Task<Result<SignUpResponse>> ExecuteAsync(SignUpRequest request)
    {
        User? savedUser = await userService.GetByEmailAsync(request.Email);

        if (savedUser != null)
            return Result.ForError<SignUpResponse>(new EmailAlredyRegisteredErrorModel());

        string hashPassword = CryptoHelper.GenerateHash(request.Password);

        User user = new(request.FullName, request.Email, hashPassword);

        await userService.CreateAsync(user);

        SignUpResponse response = new(user.Id);

        return Result.Success(response);
        
    }
}
