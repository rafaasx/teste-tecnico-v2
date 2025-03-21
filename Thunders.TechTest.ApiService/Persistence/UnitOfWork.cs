using Thunders.TechTest.ApiService.Context;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Repositories.Interfaces;

namespace Thunders.TechTest.ApiService.Persistence
{
    public class UnitOfWork(ThundersTechTestDbContext context,
                            IPedagioRepository pedagioRepository,
                            IRelatorioRepository relatorioRepository) : IUnitOfWork
    {


        public IPedagioRepository PedagioRepository => pedagioRepository;
        public IRelatorioRepository RelatorioRepository => relatorioRepository;

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose() => context.Dispose();
    }
}
