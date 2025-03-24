using Aspire.Hosting;

namespace Thunders.TechTest.Tests
{
    public class WebApplicationFixture : IAsyncLifetime
    {
        public DistributedApplication App { get; private set; }
        public HttpClient HttpClient { get; private set; }

        public async Task InitializeAsync()
        {
            var builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.Thunders_TechTest_AppHost>();

            builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            App = await builder.BuildAsync();
            await App.StartAsync();

            var resourceNotificationService =
                App.Services.GetRequiredService<ResourceNotificationService>();

            await resourceNotificationService
                .WaitForResourceAsync("apiservice", KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(30));

            HttpClient = App.CreateHttpClient("apiservice");
        }

        public async Task DisposeAsync()
        {
            if (App != null)
            {
                await App.DisposeAsync();
            }
        }
    }
}