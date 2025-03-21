using Thunders.TechTest.ApiService.Context;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Repositories.Interfaces;

namespace Thunders.TechTest.ApiService.Repositories
{
    public class PedagioRepository(ThundersTechTestDbContext context) : Repository<Pedagio>(context), IPedagioRepository
    {
    }
}
