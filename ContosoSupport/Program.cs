using ContosoSupport.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Cloud.InstrumentationFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;

namespace ContosoSupport
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            MonitoringConfig config = new(Configuration);

            IfxInitializer.IfxEnabled = true;

            IfxInitializer.IfxInitialize("ContosoAdsSupportSession");

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (UriFormatException)
            {
                // KeyVault setting not specified. You'll need to ensure that you configure this in the appsettings.json before continuing.
                throw new Exception("You must specify the Key Vault setting in the appsettings.json before continuing with the lab activity.");
            }
            catch (HttpRequestException e) when (e.Message == "No such host is known.")
            {
                // KeyVault does not exist, maybe there's a typo, maybe the KeyVault was deleted?
                throw new Exception("The Key Vault specified in the appsettings.json does not exist. Please ensure the name of the Key Vault in the appsettings.json matches the name of a Key Vault resource that currently exists in Azure.");
            }

        }

        private static IConfiguration configuration = null;

        public static IConfiguration Configuration
        {
            get
            {
                return configuration ??= new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: false)
                                .Build();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddConfiguration(Configuration);

                    var azureServiceTokenProvider = new AzureServiceTokenProvider();
                    var keyVaultClient = new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(
                            azureServiceTokenProvider.KeyVaultTokenCallback));

                    config.AddAzureKeyVault(
                        $"https://{Configuration["KeyVaultName"]}.vault.azure.net/",
                        keyVaultClient,
                        new DefaultKeyVaultSecretManager());
                })
                .ConfigureLogging((context, builder) =>
                                         builder.AddConfiguration(context.Configuration.GetSection("Logging"))
                                                .AddEventSourceLogger()
                                                .AddConsole())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //if hosting on a windows app service change Kesterl to IISIntegration.
                    webBuilder.UseUrls(Configuration.GetValue<string>("Host:Urls")).UseKestrel().UseStartup<Startup>();
                });

            return hostBuilder;
        }
    }
}
