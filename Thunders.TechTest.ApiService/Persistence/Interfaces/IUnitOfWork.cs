using Thunders.TechTest.ApiService.Repositories.Interfaces;

namespace Thunders.TechTest.ApiService.Persistence.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IPedagioRepository PedagioRepository { get; }
        IRelatorioRepository RelatorioRepository { get; }
        Task<int> SaveChangesAsync();
    }
}
