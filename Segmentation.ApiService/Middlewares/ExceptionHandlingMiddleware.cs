
namespace Segmentation.ApiService.Middlewares
{
    public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context); // выполняем следующий middleware/endpoint
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception occurred");

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var result = new
                {
                    error = "Internal Server Error",
                    details = ex.Message
                };

                await context.Response.WriteAsJsonAsync(result);
            }
        }
    }
}
