using Application.Commands.Users.Create;
using Application.Common.Exceptions;
using Application.Queries.Users.GetId;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace SecretsSharingAPI.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionHandlerMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;
            switch (ex) 
            {
                case ValidationException validationEx:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(validationEx.Errors);
                    break;
                case CreateUserException:
                    code = HttpStatusCode.BadRequest;
                    break;
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    break;
                case S3RequestException:
                    code = HttpStatusCode.BadGateway;
                    break;
                case InvalidCredentialsException:
                    code = HttpStatusCode.Unauthorized;
                    break;
                case FormatException: // Может быть брошено при чтении файла методом FromBase64String
                    code = HttpStatusCode.BadRequest;
                    break;
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            if(result == string.Empty)
            {
                result = JsonSerializer.Serialize(new { error = ex.Message });
            }

            await context.Response.WriteAsync(result);
        }
    }
}
