using Microsoft.Extensions.Options;
using Nest;
using System;

namespace Kanbersky.Ella.Business.Helpers
{
    public class ElasticClientProvider
    {
        public ElasticClientProvider(IOptions<ElasticConnectionSettings> settings)
        {
            ConnectionSettings connectionSettings = new ConnectionSettings(new Uri(settings.Value.ClusterUrl));
            connectionSettings.EnableDebugMode();

            if (settings.Value.DefaultIndex != null)
            {
                connectionSettings.DefaultIndex(settings.Value.DefaultIndex);
            }

            this.Client = new ElasticClient(connectionSettings);
        }

        public ElasticClient Client { get; set; }
    }
}
