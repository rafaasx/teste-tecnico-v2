using Rebus.Handlers;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Jobs
{
    public class PedagioHandler(IUnitOfWork unitOfWork, IPedagioService pedagioService, IMessageSender messageSender) : IHandleMessages<PedagioMessage>
    {
        public async Task Handle(PedagioMessage message)
        {
            var pedagio = new Pedagio(
                message.DataHora,
                message.Praca,
                message.Cidade,
                message.Estado,
                message.ValorPago,
                message.TipoVeiculo
            );
            var erros = pedagio.ValidarDados();
            if (erros.Any())
            {
                await messageSender.Publish(new FalhaProcessamentoPedagioMessage(message, erros));
                return;
            }
            try
            {
                await pedagioService.AddAsync(pedagio);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await messageSender.Publish(new FalhaProcessamentoPedagioMessage(message, new List<string> { "Falha no processamento do Pedágio" }, ex));
            }

        }
    }
}
