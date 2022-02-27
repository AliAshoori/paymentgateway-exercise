using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.APIs
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = Guard.Against.Null(next, nameof(next));
            _logger = Guard.Against.Null(logger, nameof(logger));
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception}");
                await HandleGlobalExceptionAsync(httpContext, exception);
            }
        }

        private static async Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                var exceptionOutput = new BadRequestResponsePayload
                {
                    Message = "Validation Failed. The payload request is not valid.",
                    Details = validationException.Errors.Select(err => new BadRequestErrorDetail
                    {
                        PropertyName = err.PropertyName,
                        AttemptedValue = err.AttemptedValue.ToString(),
                        ErrorMessage = err.ErrorMessage
                    })
                };

                var responseToWrite = JsonSerializer.Serialize(exceptionOutput, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync(responseToWrite);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                var responseToWrite = JsonSerializer.Serialize(new { exception.Message, Code = context.Response.StatusCode }, new JsonSerializerOptions { WriteIndented = true });

                await context.Response.WriteAsync(responseToWrite);
            }
        }
    }

    public static class GlobalExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
