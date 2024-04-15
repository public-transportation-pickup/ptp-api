using Microsoft.AspNetCore.Http;
using PTP.Application.GlobalExceptionHandling.Exceptions;
using System.Net;
using System.Text.Json;
using KeyNotFoundException = PTP.Application.GlobalExceptionHandling.Exceptions.KeyNotFoundException;
using NotImplementedException = PTP.Application.GlobalExceptionHandling.Exceptions.NotImplementedException;
using UnauthorizedAccessException = PTP.Application.GlobalExceptionHandling.Exceptions.UnauthorizedAccessException;
namespace PTP.Application.GlobalExceptionHandling
{
	public class GlobalErrorHandlingMiddleware : IMiddleware
	{



		private static Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			HttpStatusCode status;
			var stackTrace = String.Empty;
			string message;
			var exceptionType = exception.GetType();
			if (exceptionType == typeof(BadRequestException))
			{
				message = exception.Message;
				status = HttpStatusCode.BadRequest;
				stackTrace = exception.StackTrace;
			}
			else if (exceptionType == typeof(NotFoundException))
			{
				message = exception.Message;
				status = HttpStatusCode.NotFound;
				stackTrace = exception.StackTrace;
			}
			else if (exceptionType == typeof(NotImplementedException))
			{
				status = HttpStatusCode.NotImplemented;
				message = exception.Message;
				stackTrace = exception.StackTrace;
			}
			else if (exceptionType == typeof(UnauthorizedAccessException))
			{
				status = HttpStatusCode.Unauthorized;
				message = exception.Message;
				stackTrace = exception.StackTrace;
			}
			else if (exceptionType == typeof(KeyNotFoundException))
			{
				status = HttpStatusCode.Unauthorized;
				message = exception.Message;
				stackTrace = exception.StackTrace;
			}
			else
			{
				status = HttpStatusCode.InternalServerError;
				message = exception.Message;
				stackTrace = exception.StackTrace;
			}
			var exceptionResult = JsonSerializer.Serialize(new
			{
				error = message,
				stackTrace
			});
			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)status;
			return context.Response.WriteAsync(exceptionResult);
		}



		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				await HandleExceptionAsync(context, ex);
			}
		}
	}
}
