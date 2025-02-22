using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoSupport.Monitoring
{
    internal class MonitoringConstants
    {
        internal const string RoleName = "ContosoAdsSupport";
        internal const string CustomerResourceIdDimName = "CustomerResourceId";
        internal const string LocationDimName = "LocationId";
        internal const string LatencyMetricName = "ResponseLatencyMs";
        internal const string OperationDimName = "Operation";
        internal const string HttpStatusCodeDimName = "HttpStatusCode";
        internal const string HttpMethodName = "HttpMethod";
        internal const string TenantDimName = "Tenant";
    }
}
