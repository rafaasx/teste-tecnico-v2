using Rebus.Handlers;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Jobs
{
    public class RelatorioRankingPracasPorMesHandler(IRelatorioService relatorioService) : IHandleMessages<RelatorioRankingPracasPorMesMessage>
    {
        public async Task Handle(RelatorioRankingPracasPorMesMessage message)
        {
            await relatorioService.GerarRelatorioRankingPracasPorMes(message.Id, message.Ano, message.Quantidade);
        }
    }
}