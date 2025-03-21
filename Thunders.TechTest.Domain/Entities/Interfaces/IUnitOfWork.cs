namespace Thunders.TechTest.Domain.Entities.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedagioRepository PedagioRepository { get; }
        IRelatorioFaturamentoRepository RelatorioFaturamentoRepository { get; }
        IRelatorioTopPracasRepository RelatorioTopPracasRepository { get; }
        IRelatorioVeiculosPorPracaRepository RelatorioVeiculosPorPracaRepository { get; }
        Task<int> CommitAsync();
    }
}
