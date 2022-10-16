using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BilligKwhWebApp.Infrastructure.Logging
{
	public class RequestLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger _consoleLogger;


		public RequestLoggingMiddleware(RequestDelegate next,
										ILogger<RequestLoggingMiddleware> consoleLogger)
		{
			_next = next;
			_consoleLogger = consoleLogger;
		}

		public async Task Invoke(HttpContext context)
		{
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var startTime = DateTime.UtcNow;

			var watch = Stopwatch.StartNew();
			// Call the next delegate/middleware in the pipeline
			await _next.Invoke(context);
			watch.Stop();

			var logTemplate = @"{startTime}    {duration} ms  {clientIP}    {requestPath}";


			_consoleLogger.LogInformation(logTemplate,
				startTime,
				watch.ElapsedMilliseconds,
				context.Connection.RemoteIpAddress.ToString(),
				context.Request.Path
				);

		}
	}
}
