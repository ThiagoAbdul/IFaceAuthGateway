using IFaceAuthService.DTOs.In;
using IFaceAuthService.Extensions;
using IFaceAuthService.Helpers;
using IFaceAuthService.UseCases;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();


builder.Services.AddJwtAuthentication(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "all",
        policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
    );
});

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("all");

app.UseHealthChecks("/api/Health");

app.MapPost("/sign-in", SignIn)
.WithName("SignIn")
.WithOpenApi();

app.MapPost("/sign-up", SignUp)
.WithName("SignUp")
.WithOpenApi();

app.MapGet("protected", Protected)
.RequireAuthorization()
.WithName("Protected")
.WithOpenApi();

app.Run();

static async Task<IResult> SignIn([FromBody] SignInRequest request, SignInUseCase useCase)
{
    var result = await useCase.ExecuteAsync(request);

    if (result.HasError())
        return result.Error!.ToHttpResult();
    return TypedResults.Ok(result.Value);
}

static async Task<IResult> SignUp([FromBody] SignUpRequest request, SignUpUseCase useCase)
{
    var result = await useCase.ExecuteAsync(request);

    if (result.HasError())
        return result.Error!.ToHttpResult();
    return TypedResults.Ok(result.Value);

}

static async Task<IResult> Protected(HttpContext context, JsonWebTokenHelper tokenHelper)
{
    //string? token = context.Request.JwtToken();

    //if (string.IsNullOrEmpty(token))
    //    return TypedResults.Unauthorized();

    //var validationResult = await tokenHelper.ValidationTokenResultAsync(token);

    //if (validationResult.IsValid)
    //{
    //    return TypedResults.Ok(validationResult.SubjectId());
    //}
    //return TypedResults.Unauthorized();
    return TypedResults.Ok();

}