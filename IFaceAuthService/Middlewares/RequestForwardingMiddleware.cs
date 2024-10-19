using System.Runtime.InteropServices;

namespace IFaceAuthService.Middlewares;

public class RequestForwardingMiddleware(IHttpClientFactory httpClientFactory) : IMiddleware
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {

        if(context.Request.Path.StartsWithSegments("/auth"))
        {
            await next(context);
            return;

        }

        if (!context.User.Identity?.IsAuthenticated ?? false)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.CompleteAsync();
            return;
        }

        // Gateway
        await ForwardRequest(context);
        
    }

    private async Task ForwardRequest(HttpContext context)
    {
        var targetServiceUrl = BuilUrl(context.Request);


        var forwardRequest = new HttpRequestMessage
        {
            Method = new HttpMethod(context.Request.Method),
            RequestUri = new Uri(targetServiceUrl),
            Content = new StreamContent(context.Request.Body)
        };


        foreach (var header in context.Request.Headers)
        {
            forwardRequest.Headers.TryAddWithoutValidation(header.Key, [.. header.Value]);
        }


        var response = await _httpClient.SendAsync(forwardRequest);


        context.Response.StatusCode = (int)response.StatusCode;
        context.Response.ContentType = "application/json";
        var responseContent = await response.Content.ReadAsStringAsync();


        await context.Response.WriteAsync(responseContent);
    }

    private static string BuilUrl(HttpRequest request)
    {
        string? path = request.Path.Value;

        if(string.IsNullOrEmpty(path))
            return "";

        if (path.Contains("/api/location"))
        {
            string baseUrl = Environment
                .GetEnvironmentVariable("LOCATION_SERVICE_URL")
                ?? "http://localhost:9000";

            return baseUrl + path.Replace("/location", "");
        }

        else
        {
            string baseUrl = Environment
                .GetEnvironmentVariable("MAIN_API_URL")
                ?? "http://localhost:8080";
            return baseUrl + path;
        }
    }
}
