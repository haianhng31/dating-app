using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware 
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next; // A delegate that represents the next middleware in the pipeline
        private readonly ILogger<ExceptionMiddleware> _logger; // A logger to log exceptions
        private readonly IHostEnvironment _env; // Provides information about the hosting environment

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env) // Dependencies Injection
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // The framework expects to see a method called InvokeAsync as that's what it uses to decide what'll happen next
        public async Task InvokeAsync(HttpContext context)
        // the HttpContext is the http request that is being passed through the middleware 
        {
            try {
                await _next(context); 
                // try block: Attempts to call the next middleware in the pipeline
            } catch (Exception ex) {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json"; // we don't do this inside our API controllers 
                // because it's the default behaviour
                // but bc we're not inside the API controller rn, we need to specify that ourselves 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // this gives us a StatusCode of 500

                var response = _env.IsDevelopment() 
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json); //  Writes the JSON string to the HTTP response body.
                
            }
        }
    }
}