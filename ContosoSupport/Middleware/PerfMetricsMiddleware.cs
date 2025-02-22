using ContosoSupport.InstrumentationHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ContosoSupport.Middleware
{
    public class PerfMetricsMiddleware
    {
        private RequestDelegate Next { get; set; }
        public ILogger Logger { get; }
        private IfxMetricsHelper Metrics { get; set; }

        public PerfMetricsMiddleware(RequestDelegate next,
            ILogger<PerfMetricsMiddleware> logger,
            IServiceProvider serviceProvider)
        {
            this.Next = next;
            this.Logger = logger;

            this.Metrics = ActivatorUtilities.CreateInstance(serviceProvider, typeof(IfxMetricsHelper))
                            as IfxMetricsHelper;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Logger.LogDebug("Received {method} request for {path} on host {host}", context.Request.Method, context.Request.Path, context.Request.Host);

            var timer = Stopwatch.StartNew();
            await Next.Invoke(context).ConfigureAwait(false);
            timer.Stop();

            var action = context.Request.RouteValues["action"]?.ToString() ?? string.Empty;
            var controller = context.Request.RouteValues["controller"]?.ToString() ?? string.Empty;

            Logger.LogDebug("Logging latency metric for {action} in {controller}", action, controller);

            Metrics.RecordLatencyMeasure(timer.ElapsedMilliseconds,
                host: context.Request.Host.Host?.ToString() ?? "null",
                subscriptionId: context.Request.RouteValues["subscriptionId"]?.ToString() ?? string.Empty,
                resourceGroup: context.Request.RouteValues["resourceGroup"]?.ToString() ?? string.Empty,
                resourceId: context.Request.RouteValues["resourceId"]?.ToString() ?? string.Empty,
                controller: controller,
                action: action,
                method: context.Request.Method,
                statusCode: context.Response.StatusCode);
        }
    }
}
