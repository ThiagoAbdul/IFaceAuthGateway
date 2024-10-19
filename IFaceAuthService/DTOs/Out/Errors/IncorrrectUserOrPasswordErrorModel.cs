namespace IFaceAuthService.DTOs.Out.Errors;

public class IncorrrectUserOrPasswordErrorModel() : ErrorModel("Usuário ou senha incorretos")
{

    public override IResult ToHttpResult()
    {
        return TypedResults.Unauthorized();
    }

}
