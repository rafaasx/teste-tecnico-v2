using Newtonsoft.Json;
using Rebus.Handlers;
using Thunders.TechTest.ApiService.Messages;

namespace Thunders.TechTest.ApiService.Handlers
{
    public class FalhaProcessamentoPedagioHandler(ILogger<FalhaProcessamentoPedagioHandler> logger) : IHandleMessages<FalhaProcessamentoPedagioMessage>
    {
        public Task Handle(FalhaProcessamentoPedagioMessage message)
        {
            logger.LogError($"Não foi possível processar o pedágio: {JsonConvert.SerializeObject(message)}");
            return Task.CompletedTask;
        }
    }
}
