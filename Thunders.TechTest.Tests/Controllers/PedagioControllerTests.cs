using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.Tests.Controllers
{
    public class PedagioControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        [Fact]
        public async Task RegistrarCobranca_ShouldHandleHighVolumeRequests()
        {
            var builder = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.Thunders_TechTest_AppHost>();

            builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
            {
                clientBuilder.AddStandardResilienceHandler();
            });

            await using var app = await builder.BuildAsync();

            await app.StartAsync();

            var httpClient = app.CreateHttpClient("apiservice");

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

            var resourceNotificationService =
                app.Services.GetRequiredService<ResourceNotificationService>();
            await resourceNotificationService
                .WaitForResourceAsync("apiservice", KnownResourceStates.Running)
                .WaitAsync(TimeSpan.FromSeconds(30));


            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 1000; i++)
            {
                var valor = new Random().Next(1, 15);
                var tipoVeciculo = new Random().Next(0, 2);
                var request = new PedagioMessage(DateTime.UtcNow, "Praça A", "Cidade", "SC", valor, (TipoVeiculoEnum)tipoVeciculo);
                tasks.Add(httpClient.PostAsJsonAsync("/api/Pedagio", request));
                Thread.Sleep(300);
            }

            try
            {
                var responses = await Task.WhenAll(tasks);
                foreach (var response in responses)
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
