namespace IFaceAuthService.DTOs.Out.Errors;

public class EmailAlredyRegisteredErrorModel() : ErrorModel("Email já cadastrado")
{
    public override IResult ToHttpResult()
    {
        return TypedResults.BadRequest(Message);
    }
}
