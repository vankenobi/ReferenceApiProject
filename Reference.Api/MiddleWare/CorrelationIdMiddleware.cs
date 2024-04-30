using System;
using Serilog.Context;

namespace Reference.Api.MiddleWare
{
	public class CorrelationIdMiddleware
	{
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers["CorrelationId"].FirstOrDefault();

            if (string.IsNullOrEmpty(correlationId))
            {
                LogContext.PushProperty("CorrelationId", context.TraceIdentifier);
            }

            await _next(context);
        }

    }
}

