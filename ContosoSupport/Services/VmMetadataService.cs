using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ContosoSupport.Services
{
    public class VmMetadataService : IVmMetadataService
    {
        const string apiVersion = "2019-06-04";
        const string metadataHeaderName = "Metadata";
        const string metadataHeaderValue = "true";
        const string computeObjectName = "compute";
        const string locationPropertyName = "location";

        public async Task<string> GetComputeLocationAsync(string defaultRegion = "localhost")
        {
            Uri endpointUri = new Uri($"http://169.254.169.254/metadata/instance?api-version={apiVersion}");

            using var client = new HttpClient();

            client.DefaultRequestHeaders.Add(metadataHeaderName, metadataHeaderValue);

            HttpResponseMessage response;

            try
            {
                response = await client.GetAsync(endpointUri).ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                // We're not running in Azure right now
                return defaultRegion;
            }

            using var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return ExtractLocation(await JsonDocument.ParseAsync(responseStream).ConfigureAwait(false)) ?? defaultRegion;
        }

        private string ExtractLocation(JsonDocument json)
        {
            if (!json.RootElement.TryGetProperty(computeObjectName, out var computeObject)) return null;
            if (!computeObject.TryGetProperty(locationPropertyName, out var locationProperty)) return null;

            return locationProperty.GetString();
        }
    }
}
