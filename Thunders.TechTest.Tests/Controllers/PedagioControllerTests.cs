using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Enums;
using Thunders.TechTest.ApiService.Messages;

namespace Thunders.TechTest.Tests.Controllers
{
    public class PedagioControllerTests(WebApplicationFixture fixture) : IClassFixture<WebApplicationFixture>
    {
        [Fact]
        public async Task RegistrarCobranca_ShouldHandleHighVolumeRequests()
        {
            var tasks = new List<Task<HttpResponseMessage>>();

            for (int i = 0; i < 1000; i++)
            {
                var valor = new Random().Next(1, 15);
                var tipoVeciculo = new Random().Next(0, 2);
                var request = new PedagioMessage(DateTime.UtcNow, "Praça A", "Cidade", "SC", valor, (TipoVeiculoEnum)tipoVeciculo);
                tasks.Add(fixture.HttpClient.PostAsJsonAsync("/api/Pedagio", request));
                Thread.Sleep(200);
            }

            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
