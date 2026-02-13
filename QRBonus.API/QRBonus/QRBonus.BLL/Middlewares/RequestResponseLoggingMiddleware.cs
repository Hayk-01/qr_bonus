using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace QRBonus.BLL.Middlewares;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log the request
        var request = await FormatRequest(context.Request);

        // Copy the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            // Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            // Log the response
            var response = await FormatResponse(context.Response, context.Request.Path);
            _logger.LogInformation("HTTP Request: {@Request}, HTTP Response: {@Response}", request, response);

            // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<object> FormatRequest(HttpRequest request)
    {
        string? bodyAsText = string.Empty;

        if (!_ignoreRequestBodyPaths.Contains(request.Path.ToString().ToLower()))
        {
            request.EnableBuffering();
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin); // Rewind the stream after reading
        }

        return new
        {
            Scheme = request.Scheme,
            Host = request.Host.Value,
            Path = request.Path,
            Headers = request.Headers.ToDictionary(x => x.Key, x => x.Value),
            QueryString = request.QueryString.Value,
            RequestBody = bodyAsText
        };
    }

    private async Task<object> FormatResponse(HttpResponse response, string requestPath)
    {
        if (_ignoreResponseBodyPaths.Contains(requestPath.ToLower()))
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            return new
            {
                StatusCode = response.StatusCode,
                ResponseBody = string.Empty
            };
        }

        response.Body.Seek(0, SeekOrigin.Begin);
        var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
        response.Body.Seek(0, SeekOrigin.Begin); // Rewind the stream after reading

        return new
        {
            StatusCode = response.StatusCode,
            ResponseBody = bodyAsText
        };
    }

    private readonly List<string> _ignoreRequestBodyPaths = new List<string>();

    private readonly List<string> _ignoreResponseBodyPaths = new List<string>();
}