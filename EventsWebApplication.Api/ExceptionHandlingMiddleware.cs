using System.Net;
using System.Text.Json;

namespace EventsWebApplication.Api
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); 
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case ValidationException validationException:
                    status = HttpStatusCode.BadRequest;
                    message = validationException.Message;
                    break;
                case KeyNotFoundException keyNotFound:
                    status = HttpStatusCode.NotFound;
                    message = keyNotFound.Message;
                    break;
                case InvalidOperationException invalidOperation:
                    status = HttpStatusCode.NotFound;
                    message = invalidOperation.Message;
                    break;
                case ArgumentNullException argumentNull:
                    status = HttpStatusCode.NotFound;
                    message = argumentNull.Message;
                    break;
                case Exception exception1:
                    status = HttpStatusCode.ExpectationFailed;
                    message = exception1.Message;
                    break;
                default:
                    status= HttpStatusCode.InternalServerError;
                    message = "An occurad error";
                    break;
            }

            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(result);
        }
    }

    public class ValidationException : Exception
    {
        public List<string> Errors { get; }

        public ValidationException(List<string> errors)
            : base("Validation failed")
        {
            Errors = errors;
        }
    }
}
