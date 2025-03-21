using Rebus.Handlers;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Handlers
{
    public class FalhaProcessamentoRelatorioHandler(ILogger<FalhaProcessamentoRelatorioHandler> logger, IRelatorioService relatorioService, IUnitOfWork unitOfWork) : IHandleMessages<FalhaProcessamentoRelatorioMessage>
    {
        public async Task Handle(FalhaProcessamentoRelatorioMessage message)
        {
            logger.LogError($"Relatório {message.Id} não pode ser processado: {message.Exception.Message}");
            var relatorio = await relatorioService.GetByIdAsync(message.Id);
            if (relatorio is null)
                return;
            relatorio.ProcessarComErro();
            await unitOfWork.SaveChangesAsync();
        }
    }
}
