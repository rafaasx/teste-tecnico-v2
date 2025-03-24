using Newtonsoft.Json;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repositories;
using Thunders.TechTest.ApiService.Response;

namespace Thunders.TechTest.ApiService.Factory
{
    public static class RelatorioResponseFactory
    {
        public static RelatorioResponse CreateRelatorioResponse(Relatorio relatorio)
        {
            if (string.IsNullOrWhiteSpace(relatorio.Dados))
                return new RelatorioResponse(relatorio.Id, relatorio.Status);
            switch (relatorio.TipoRelatorio)
            {
                case Enums.TipoRelatorioEnum.ValorTotalPorHora:
                    return new RelatorioResponse(relatorio.Id, relatorio.Status, JsonConvert.DeserializeObject<List<RelatorioValorTotalPorHoraResponse>>(relatorio.Dados!));
                case Enums.TipoRelatorioEnum.RankingPracasPorMes:
                    return new RelatorioResponse(relatorio.Id, relatorio.Status, JsonConvert.DeserializeObject<List<RelatorioRankingPracasPorMesResponse>>(relatorio.Dados!));
                case Enums.TipoRelatorioEnum.QuantidadeTiposVeiculoPorPraca:
                    return new RelatorioResponse(relatorio.Id, relatorio.Status, JsonConvert.DeserializeObject<RelatorioQuantidadeDeTiposDeVeiculosPorPracaResponse>(relatorio.Dados!));
                default:
                    return new RelatorioResponse(relatorio.Id, relatorio.Status);
            }
        }
    }
}
