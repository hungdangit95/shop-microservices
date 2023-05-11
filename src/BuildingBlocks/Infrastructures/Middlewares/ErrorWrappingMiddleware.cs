using Microsoft.AspNetCore.Http;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            var errorMsg = string.Empty;

            try
            {
                await _next.Invoke(context);
            }
            catch (ValidationException ex)
            {
                throw ex;
            }
        }
    }
}
