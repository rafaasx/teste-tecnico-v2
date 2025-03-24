using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Diagnostics;
using System.Net;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Request;
using Thunders.TechTest.ApiService.Response;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedagioController(IMessageSender messageSender) : ControllerBase
    {
        [HttpPost]
        [SwaggerOperation(
            Summary = "Registrar passagem pelo pedágio",
            Description = "Este endpoint registra uma passagem pelo pedágio e retorna um identificador único.",
            OperationId = "RegistrarPassagem",
            Tags = new[] { "Pedágios" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Passagem registrada com sucesso", typeof(PedagioResponse))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Requisição inválida")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro interno do servidor")]
        public async Task<IActionResult> RegistrarPassagem([FromBody] PedagioRequest request)
        {
            try
            {
                var message = new PedagioMessage(request.DataHora, request.Praca, request.Cidade, request.Estado, request.ValorPago, request.TipoVeiculo, Guid.NewGuid());
                Debug.WriteLine($"{DateTime.Now:HH:mm:ss.fff} - Enviando mensagem: {message.Id}");
                await messageSender.Publish(message);
                return Ok(new PedagioResponse(message.Id));
            }
            catch (Exception ex)
            {
                return Problem($"Erro ao processar a solicitação: {ex.Message}");
            }
        }
    }
}
