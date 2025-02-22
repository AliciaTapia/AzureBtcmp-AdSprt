using ContosoSupport.Middleware;
using ContosoSupport.Monitoring;
using ContosoSupport.Services;
using Microsoft.Cloud.InstrumentationFramework.Metrics.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;

namespace ContosoSupport.InstrumentationHelpers
{
    public sealed class IfxMetricsHelper : IDisposable
    {
        private readonly MonitoringConfig config;
        private readonly string location;
        private readonly ILogger logger;
        private IMdmCumulativeMetric<DimensionValues6D, ulong> latencyMeasure;
        private string tenantId;

        public IfxMetricsHelper(IConfiguration config,
            IVmMetadataService vmMetadataService,
            ILogger<IfxMetricsHelper> logger)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (vmMetadataService is null)
            {
                throw new ArgumentNullException(nameof(vmMetadataService));
            }

            this.config = new MonitoringConfig(config);
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.location = vmMetadataService.GetComputeLocationAsync().Result;

            if (!MdmMetricController.StartMetricPublication())
            {
                logger.LogWarning("Unable to start metric publication");
            }

            CreateLatencyMeasure();
        }

        private void CreateLatencyMeasure()
        {
            var metricFactory = new MdmMetricFactory();

            var latencyBehavior = new MdmBucketedDistributionBehavior()
            {
                MinimumValue = this.config.Behavior.MinimumValue,
                BucketSize = this.config.Behavior.BucketSize,
                BucketCount = this.config.Behavior.BucketCount
            };

            // Region, TenantId, CRID, HttpMethod, HttpStatusCode, OperationId
            latencyMeasure = metricFactory.CreateUInt64CumulativeDistributionMetric(
                flags: MdmMetricFlags.CumulativeMetricDefault,
                distributionBehavior: latencyBehavior,
                monitoringAccount: this.config.Account,
                metricNamespace: this.config.Namespace,
                metricName: MonitoringConstants.LatencyMetricName,
                dim1: MonitoringConstants.LocationDimName,
                dim2: MonitoringConstants.TenantDimName,
                dim3: MonitoringConstants.CustomerResourceIdDimName,
                dim4: MonitoringConstants.HttpMethodName,
                dim5: MonitoringConstants.HttpStatusCodeDimName,
                dim6: MonitoringConstants.OperationDimName
            );

            if (null == latencyMeasure)
            {
                logger.LogWarning("Failed to create {measure}", nameof(latencyMeasure));
            }
        }

        public void Dispose()
        {
            MdmMetricController.FlushAndStopMetricPublication(TimeSpan.FromSeconds(config.ShutdownDurationSeconds));
        }

        public void RecordLatencyMeasure(long measure,
            string host,
            string subscriptionId,
            string resourceGroup,
            string resourceId,
            string method,
            int statusCode,
            string controller,
            string action
            )
        {
            if (!this.config.HasLoaded)
            {
                logger.LogDebug("Monitoring configuration not loaded, skipping latency logging for {action} request on {controller}",
                    action, controller);
                return;
            }

            #region Construct Tenant, CRID, and OperationId values
            // Tenant would typically refer to a deployment, here we're generating a sample value
            string tenant = tenantId ?? (tenantId = $"{location}PrdCSP{host?.Split('.', StringSplitOptions.RemoveEmptyEntries)[0]}");
            
            string customerResourceId = !string.IsNullOrWhiteSpace(subscriptionId)
                                            ? $"subscriptions/{subscriptionId}/resourceGroups/{resourceGroup}/providers/Contoso.Support/ticketingSystem/{resourceId}"
                                            : "null";
            string operationId = !string.IsNullOrWhiteSpace(controller) ? $"{controller}.{action}" : "null";
            #endregion

            var dimensionValues = DimensionValues.Create(
                location,
                tenant,
                customerResourceId,
                method,
                statusCode.ToString(CultureInfo.InvariantCulture),
                operationId
            );

            var success = latencyMeasure?.Set((ulong)measure, dimensionValues) ?? false;

            if (!success)
            {
                logger.LogWarning("Failed to record {measure}", nameof(latencyMeasure));
            }


        }

    }
}
