using Thunders.TechTest.ApiService.Context;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repositories.Interfaces;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class RelatorioRepository(ThundersTechTestDbContext context) : Repository<Relatorio>(context), IRelatorioRepository
    {
    }
}
