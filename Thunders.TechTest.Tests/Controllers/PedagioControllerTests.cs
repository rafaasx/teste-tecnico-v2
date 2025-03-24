using System.Net.Http.Json;
using Thunders.TechTest.ApiService.Enums;
using Thunders.TechTest.ApiService.Messages;
using System.Net;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.Tests.Controllers
{
    public class PedagioControllerTests(WebApplicationFixture fixture) : IClassFixture<WebApplicationFixture>
    {
        public static List<string> Cidades => new List<string> { "Joinville", "Brasília", "São Paulo", "Rio de Janeiro", "Porto Alegre", "Curitiba" };
        public static List<string> Pracas => new List<string> { "Praça A", "Praça B", "Praça C", "Praça D" };

        [Fact]
        public async Task RegistrarCobranca_Processar_Um_Grande_Volume_De_Mensagens()
        {
            var limiteRequisicoesParalelas = 20;
            var semaforo = new SemaphoreSlim(limiteRequisicoesParalelas);
            await Parallel.ForEachAsync(Enumerable.Range(0, 1000), async (_, _) =>
            {
                await semaforo.WaitAsync();
                try
                {
                    var valor = new Random().Next(1, 15);
                    var tipoVeiculo = new Random().Next(0, 2);
                    var cidade = Cidades[new Random().Next(0, Cidades.Count)];
                    var praca = Pracas[new Random().Next(0, Pracas.Count)];
                    var request = new PedagioMessage(DateTime.UtcNow, praca, cidade, "DF", valor, (TipoVeiculoEnum)tipoVeiculo, Guid.NewGuid());
                    var response = await fixture.HttpClient.PostAsJsonAsync("/api/Pedagio", request);
                    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                }
                finally
                {
                    semaforo.Release();
                }
            });
        }
    }
}
