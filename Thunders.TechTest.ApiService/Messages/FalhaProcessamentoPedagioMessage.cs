namespace Thunders.TechTest.ApiService.Messages
{
    public record FalhaProcessamentoPedagioMessage(object Message, List<string> erros, Exception? Exception = null);

}
