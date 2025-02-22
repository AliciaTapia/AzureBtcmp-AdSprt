using Microsoft.Extensions.Configuration;

namespace ContosoSupport.Middleware
{
    internal class MonitoringConfig
    {
        public MonitoringConfig(IConfiguration config)
        {
            IConfigurationSection monitoringSection;

            if (null != (monitoringSection = config.GetSection("Monitoring")))
            {
                // Load the monitoring settings from config
                monitoringSection.Bind(this);

                // Reset the loaded status after binding to config, it will be refreshed when next checked
                hasLoaded = null;
            }
        }

        private bool? hasLoaded = null;
        public bool HasLoaded
        {
            get
            {
                 return (hasLoaded.HasValue ? hasLoaded : hasLoaded =
                      (!string.IsNullOrWhiteSpace(Account)
                    && !string.IsNullOrWhiteSpace(Namespace)
                    && !string.IsNullOrWhiteSpace(Tenant)
                    && (null != Behavior))).Value;
            }
        }
        public string Account { get; set; }
        public string Namespace { get; set; }
        public string Tenant { get; set; }
        public int ShutdownDurationSeconds { get; set; }
        public BehaviorConfig Behavior { get; set; }
    }

}