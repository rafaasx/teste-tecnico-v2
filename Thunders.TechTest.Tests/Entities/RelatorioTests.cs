using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.Tests.Entities
{
    public class RelatorioTests
    {
        [Fact]
        public void ChangeDados_ShouldUpdateDados()
        {
            var relatorio = new Relatorio(TipoRelatorioEnum.ValorTotalPorHora, "parametros");
            var newDados = "new dados";

            relatorio.ChangeDados(newDados);

            Assert.Equal(newDados, relatorio.Dados);
        }

        [Fact]
        public void Processar_ShouldUpdateStatusToProcessado()
        {
            var relatorio = new Relatorio(TipoRelatorioEnum.ValorTotalPorHora, "parametros");

            relatorio.Processar();

            Assert.Equal(StatusRelatorioEnum.Processado, relatorio.Status);
        }

        [Fact]
        public void ProcessarComErro_ShouldUpdateStatusToErro()
        {
            var relatorio = new Relatorio(TipoRelatorioEnum.ValorTotalPorHora, "parametros");

            relatorio.ProcessarComErro();

            Assert.Equal(StatusRelatorioEnum.Erro, relatorio.Status);
        }
    }
}