using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Dtos;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedagioController(IMessageSender messageSender) : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> RegistrarPassagem([FromBody] PedagioMessage request)
        {
            try
            {
                await messageSender.Publish(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem($"Erro ao processar a solicitação: {ex.Message}");
            }
        }
    }
}
