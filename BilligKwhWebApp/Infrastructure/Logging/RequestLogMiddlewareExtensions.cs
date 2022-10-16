using Microsoft.AspNetCore.Builder;

namespace BilligKwhWebApp.Infrastructure.Logging
{
	public static class RequestLogggingMiddlewareExtensions
	{
		public static IApplicationBuilder UseRequestLogging(
			this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<RequestLoggingMiddleware>();
		}
	}
}
