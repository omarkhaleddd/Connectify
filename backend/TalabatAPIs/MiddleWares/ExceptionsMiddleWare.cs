using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
    public class ExceptionsMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionsMiddleWare> _logger;
        private readonly IHostEnvironment _environment;

        public ExceptionsMiddleWare(RequestDelegate Next,ILogger<ExceptionsMiddleWare> logger,IHostEnvironment environment)
        {
            _next = Next;
           _logger = logger;
            _environment = environment;
        }
        public async Task InvokeAsync(HttpContext context )
        {
            try
            {
              await  _next.Invoke(context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //if (_environment.IsDevelopment())
                //{
                //    var Response = new ApiExceptionsResponse(500, ex.Message, ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiExceptionsResponse((int)HttpStatusCode.InternalServerError);

                //}
                var Response = _environment.IsDevelopment() ? new ApiExceptionsResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExceptionsResponse((int)HttpStatusCode.InternalServerError);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
               };
                var jsonResponse = JsonSerializer.Serialize(Response,options);
                context.Response.WriteAsync(jsonResponse);
            }
        }
    }
}
