using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Services.Implementations
{
    public class PedagioService : IPedagioService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PedagioService(IUnitOfWork unitOfWork) { _unitOfWork = unitOfWork; }

        public async Task AddAsync(Pedagio pedagio)
        {
            if (!pedagio.ValidarDados().Any())
            {
                await _unitOfWork.PedagioRepository.AddAsync(pedagio);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
