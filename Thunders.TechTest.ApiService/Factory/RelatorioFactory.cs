using Newtonsoft.Json;
using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Factory
{
    public static class RelatorioFactory
    {
        public static Relatorio CriarRelatorioRankingPracasPorMes(int ano, int quantidade)
        {
            return new Relatorio(Enums.TipoRelatorioEnum.RankingPracasPorMes, JsonConvert.SerializeObject(new { ano, quantidade }));
        }
        public static Relatorio CriarRelatorioValorTotalPorHora(DateTime periodoInicial, DateTime periodoFinal)
        {
            return new Relatorio(Enums.TipoRelatorioEnum.ValorTotalPorHora, JsonConvert.SerializeObject(new { periodoInicial, periodoFinal }));
        }

        public static Relatorio CriarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(string praca)
        {
            return new Relatorio(Enums.TipoRelatorioEnum.ValorTotalPorHora, JsonConvert.SerializeObject(new { praca }));

        }
    }
}
