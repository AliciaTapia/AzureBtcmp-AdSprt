using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ContosoSupport.Middleware
{
    public class LatencyInjectionMiddleware
    {
        private RequestDelegate Next { get; set; }

        public ILogger Logger { get; }

        private readonly Random random = new();

        public LatencyInjectionMiddleware(RequestDelegate next,
            ILogger<LatencyInjectionMiddleware> logger)
        {
            this.Next = next;
            this.Logger = logger;
        }

        private int GenerateLatency()
        {
            double anomalyPct = 0.5;
            var isAnomaly = ((random.Next(0, 10000) / 10000.00) * 100.00) < anomalyPct;

            int floor = !isAnomaly ? 0 : 500;
            int ceil = !isAnomaly ? 500 : 2000;

            return random.Next(floor, ceil);
        }

        public async Task Invoke(HttpContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            int latency = GenerateLatency();

            Logger.LogDebug("Adding {latency}ms latency to {method} request for {path}", latency, context.Request.Method, context.Request.Path);

            await Task.Delay(latency).ConfigureAwait(false);
            await Next.Invoke(context).ConfigureAwait(false);

        }
    }
}
