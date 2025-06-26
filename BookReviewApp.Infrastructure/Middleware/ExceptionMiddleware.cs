using BookReviewApp.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BookReviewApp.Infrastructure.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                if (context.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleApiExceptionAsync(context, ex);
                }
                else
                {
                    // Let the pipeline continue to MVC error page
                    context.Response.Redirect("/Home/Error");
                }
            }
        }

        private static Task HandleApiExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problem = new ProblemDetails
            {
                Title = "An error occurred while processing your request.",
                Detail = exception.Message,
                Status = context.Response.StatusCode,
                Type = $"https://httpstatuses.com/{context.Response.StatusCode}",
                Instance = context.TraceIdentifier
            };

            var result = JsonSerializer.Serialize(problem);
            return context.Response.WriteAsync(result);
        }
    }

}
