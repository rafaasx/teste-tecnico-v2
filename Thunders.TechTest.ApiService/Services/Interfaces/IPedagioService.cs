using Thunders.TechTest.ApiService.Entities;

namespace Thunders.TechTest.ApiService.Services.Interfaces
{
    public interface IPedagioService
    {
        Task AddAsync(Pedagio pedagio);
    }
}
