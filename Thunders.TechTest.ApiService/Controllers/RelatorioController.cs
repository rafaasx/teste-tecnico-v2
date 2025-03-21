using Microsoft.AspNetCore.Mvc;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Factory;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Request;
using Thunders.TechTest.ApiService.Response;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioController(IMessageSender messageSender, IRelatorioService relatorioService, IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IMessageSender _messageSender = messageSender;

        [HttpPost("processar-relatorio-ranking-pracas-por-mes")]
        public async Task<IActionResult> ProcessarRelatorioRankingPracasPorMes([FromBody] RelatorioRankingPracasPorMesRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioRankingPracasPorMes(request.Ano, request.Quantidade);
                var message = new RelatorioRankingPracasPorMesMessage(relatorio.Id, request.Ano, request.Quantidade);

                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await _messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório de ranking de praças por mês: {ex.Message}");
            }
        }
        [HttpPost("processar-relatorio-valor-total-por-hora")]
        public async Task<IActionResult> ProcessarRelatorioValorTotalPorHora([FromBody] RelatorioValorTotalPorHoraRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioValorTotalPorHora(request.PeriodoInicial, request.PeriodoFinal);
                var message = new RelatorioValorTotalPorHoraMessage(relatorio.Id, request.PeriodoInicial, request.PeriodoFinal);
                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await _messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório valor total por hora: {ex.Message}");
            }
        }
        [HttpPost("processar-relatorio-quantidade-de-tipos-de-veiculos-por-praca")]
        public async Task<IActionResult> ProcessarRelatorioQuantidadeDeTiposDeVeiculosPorPraca([FromBody] RelatorioQuantidadeDeTiposDeVeiculosPorPracaRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(request.Praca);
                var message = new RelatorioQuantidadeDeTiposDeVeiculosPorPracaMessage(relatorio.Id, request.Praca);
                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await _messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório quantidade de tipos de veiculos por praça: {ex.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var relatorio = await relatorioService.GetByIdAsync(id);
                if (relatorio == null)
                    return NotFound();

                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status, relatorio.Dados));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o relatório: {ex.Message}");
            }
        }
    }
}
