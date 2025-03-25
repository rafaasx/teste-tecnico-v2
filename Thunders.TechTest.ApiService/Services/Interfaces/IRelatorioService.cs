using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IRelatorioService
    {
        Task GerarRelatorioValorTotalPorHora(Guid id, DateTime periodoInicial, DateTime periodoFinal);
        Task GerarRelatorioRankingPracasPorMes(Guid id, int Ano, int quantidade);
        Task GerarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(Guid id, string praca);
        Task<Relatorio?> GetByIdAsync(Guid id);
        Task AddAsync(Relatorio relatorio);
    }
}
