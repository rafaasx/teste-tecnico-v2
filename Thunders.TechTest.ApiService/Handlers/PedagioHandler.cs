using Rebus.Handlers;
using System.Diagnostics;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Handlers
{
    public class PedagioHandler(IUnitOfWork unitOfWork, IPedagioService pedagioService, IMessageSender messageSender) : IHandleMessages<PedagioMessage>
    {
        public async Task Handle(PedagioMessage message)
        {
            try
            {
                Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Processando mensagem: {message.Id}");
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
                await pedagioService.AddAsync(pedagio);
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await messageSender.Publish(new FalhaProcessamentoPedagioMessage(message, new List<string> { "Falha no processamento do Pedágio" }, ex));
            }
            finally
            {
                Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Mensagem processada: {message.Id}");

            }
        }
    }
}
