using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.Tests.Controllers
{
    public class PedagioControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PedagioControllerTests(WebApplicationFactory<Thunders.TechTest.ApiService.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RegistrarCobranca_ShouldHandleHighVolumeRequests()
        {
            // Arrange
            var client = _factory.CreateClient();
            var request = new PedagioMessage(DateTime.UtcNow, "Praça A","Cidade", "Estado", 10, TipoVeiculoEnum.Caminhao);

            var tasks = new List<Task<HttpResponseMessage>>();

            // Act
            for (int i = 0; i < 1000; i++) // Simulando 1000 requisições
            {
                tasks.Add(client.PostAsJsonAsync("/api/Pedagio", request));
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            foreach (var response in responses)
            {
                Assert.True(response.IsSuccessStatusCode, "A requisição falhou.");
            }
        }
    }
}
