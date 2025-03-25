using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Response;
using Thunders.TechTest.ApiService.Services.Interfaces;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.ApiService.Services.Implementations
{
    public class RelatorioService(IUnitOfWork unitOfWork, IMessageSender messageSender) : IRelatorioService
    {
        public async Task GerarRelatorioValorTotalPorHora(Guid id, DateTime periodoInicial, DateTime periodoFinal)
        {
            var relatorio = unitOfWork.RelatorioRepository.Query().FirstOrDefault(r => r.Id == id);
            if (relatorio is null)
                return;

            var resultado = await unitOfWork.PedagioRepository.Query()
                .Where(p => p.DataHora.Date >= periodoInicial.Date && p.DataHora.Date <= periodoFinal.Date)
                .GroupBy(p => new { p.Cidade, p.DataHora.Hour })
                .Select(g => new RelatorioValorTotalPorHoraResponse(g.Key.Cidade, g.Key.Hour, g.Sum(x => x.ValorPago)))
                .ToListAsync();

            if (!resultado.Any())
                return;

            relatorio.ChangeDados(JsonConvert.SerializeObject(resultado));
            relatorio.Processar();
            await unitOfWork.SaveChangesAsync();
        }

        public async Task GerarRelatorioRankingPracasPorMes(Guid id, int ano, int quantidade)
        {
            try
            {
                var relatorio = unitOfWork.RelatorioRepository.Query().FirstOrDefault(r => r.Id == id);
                if (relatorio is null)
                    return;
                var query = await unitOfWork.PedagioRepository.Query()
                     .Where(p => p.DataHora.Year == ano)
                     .GroupBy(p => new { p.DataHora.Year, p.DataHora.Month, p.Praca })
                     .Select(g => new
                     {
                         Ano = g.Key.Year,
                         Mes = g.Key.Month,
                         Praca = g.Key.Praca,
                         TotalFaturado = g.Sum(p => p.ValorPago)
                     })
                     .OrderBy(q => q.Ano)
                     .ThenBy(q => q.Mes)
                     .ThenByDescending(q => q.TotalFaturado)
                     .ToListAsync();
                if (!query.Any())
                    return;
                var resultado = query
                    .GroupBy(q => new { q.Ano, q.Mes })
                    .SelectMany(g => g.OrderByDescending(p => p.TotalFaturado)
                                      .Take(quantidade))
                    .Select(q => new RelatorioRankingPracasPorMesResponse(
                        q.Ano,
                        q.Mes,
                        q.Praca,
                        q.TotalFaturado
                    ))
                    .ToList();

                relatorio.ChangeDados(JsonConvert.SerializeObject(resultado));
                relatorio.Processar();
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await messageSender.Publish(new FalhaProcessamentoRelatorioMessage(id, ex));
            }
        }

        public async Task GerarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(Guid id, string praca)
        {
            try
            {
                var relatorio = unitOfWork.RelatorioRepository.Query().FirstOrDefault(r => r.Id == id);
                if (relatorio is null)
                    return;

                var resultado = unitOfWork.PedagioRepository.Query()
                .Where(p => p.Praca == praca)
                .Select(p => p.TipoVeiculo)
                .Distinct()
                .Count();

                relatorio.ChangeDados(JsonConvert.SerializeObject(new RelatorioQuantidadeDeTiposDeVeiculosPorPracaResponse(resultado)));
                relatorio.Processar();
                await unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await messageSender.Publish(new FalhaProcessamentoRelatorioMessage(id, ex));

            }
        }

        public async Task<Relatorio?> GetByIdAsync(Guid id)
        {
            var relatorio = await unitOfWork.RelatorioRepository.Query().FirstOrDefaultAsync(r => r.Id == id);
            return relatorio;
        }

        public async Task AddAsync(Relatorio relatorio)
        {
            await unitOfWork.RelatorioRepository.AddAsync(relatorio);
        }
    }
}
