using Rebus.Handlers;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Jobs
{
    public class RelatorioQuantidadeDeTiposDeVeiculosPorPracaHandler(IRelatorioService relatorioService) : IHandleMessages<RelatorioQuantidadeDeTiposDeVeiculosPorPracaMessage>
    {
        public async Task Handle(RelatorioQuantidadeDeTiposDeVeiculosPorPracaMessage message)
        {
            await relatorioService.GerarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(message.Id, message.Praca);
        }
    }
}