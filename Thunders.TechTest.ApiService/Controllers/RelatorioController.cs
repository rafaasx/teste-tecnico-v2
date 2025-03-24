using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
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
        [HttpPost("processar-relatorio-ranking-pracas-por-mes")]
        [SwaggerOperation(
            Summary = "Inicia o processamento do relatório de ranking de praças por mês",
            Description = "Este endpoint processa o relatório de ranking de praças baseado no ano informado.",
            OperationId = "ProcessarRelatorioRankingPracasPorMes",
            Tags = new[] { "Relatórios" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Relatório iniciado com sucesso", typeof(RelatorioResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro interno ao processar o relatório")]
        public async Task<IActionResult> ProcessarRelatorioRankingPracasPorMes([FromBody] RelatorioRankingPracasPorMesRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioRankingPracasPorMes(request.Ano, request.Quantidade);
                var message = new RelatorioRankingPracasPorMesMessage(relatorio.Id, request.Ano, request.Quantidade);

                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório de ranking de praças por mês: {ex.Message}");
            }
        }

        [HttpPost("processar-relatorio-valor-total-por-hora")]
        [SwaggerOperation(
            Summary = "Inicia o processamento do relatório de valor total por hora",
            Description = "Este endpoint processa o relatório de faturamento total por hora dentro do período informado.",
            OperationId = "ProcessarRelatorioValorTotalPorHora",
            Tags = new[] { "Relatórios" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Relatório iniciado com sucesso", typeof(RelatorioResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro interno ao processar o relatório")]
        public async Task<IActionResult> ProcessarRelatorioValorTotalPorHora([FromBody] RelatorioValorTotalPorHoraRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioValorTotalPorHora(request.PeriodoInicial, request.PeriodoFinal);
                var message = new RelatorioValorTotalPorHoraMessage(relatorio.Id, request.PeriodoInicial, request.PeriodoFinal);
                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório valor total por hora: {ex.Message}");
            }
        }

        [HttpPost("processar-relatorio-quantidade-de-tipos-de-veiculos-por-praca")]
        [SwaggerOperation(
            Summary = "Inicia o processamento do relatório de quantidade de tipos de veículos por praça",
            Description = "Este endpoint processa um relatório que contabiliza os tipos de veículos em uma praça específica.",
            OperationId = "ProcessarRelatorioQuantidadeDeTiposDeVeiculosPorPraca",
            Tags = new[] { "Relatórios" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Relatório iniciado com sucesso", typeof(RelatorioResponse))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro interno ao processar o relatório")]
        public async Task<IActionResult> ProcessarRelatorioQuantidadeDeTiposDeVeiculosPorPraca([FromBody] RelatorioQuantidadeDeTiposDeVeiculosPorPracaRequest request)
        {
            try
            {
                var relatorio = RelatorioFactory.CriarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(request.Praca);
                var message = new RelatorioQuantidadeDeTiposDeVeiculosPorPracaMessage(relatorio.Id, request.Praca);
                await unitOfWork.RelatorioRepository.AddAsync(relatorio);
                await unitOfWork.SaveChangesAsync();
                await messageSender.Publish(message);
                return Ok(new RelatorioResponse(relatorio.Id, relatorio.Status));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao iniciar o processamento do relatório quantidade de tipos de veiculos por praça: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Recupera um relatório pelo identificador",
            Description = "Este endpoint retorna o cadastro de um relatório com seus dados e status de processamento",
            OperationId = "ProcessarRelatorioQuantidadeDeTiposDeVeiculosPorPraca",
            Tags = new[] { "Relatórios" }
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Relatório iniciado com sucesso", typeof(RelatorioResponse))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Erro relatório não encontrado")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro interno ao processar o relatório")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            try
            {
                var relatorio = await relatorioService.GetByIdAsync(id);
                if (relatorio == null)
                    return NotFound();
                return Ok(RelatorioResponseFactory.CreateRelatorioResponse(relatorio));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao obter o relatório: {ex.Message}");
            }
        }
    }
}
