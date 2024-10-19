namespace IFaceAuthService.DTOs.Out;

public record SignInResponse(Guid UserId, string AccessToken, string RefreshToken);
