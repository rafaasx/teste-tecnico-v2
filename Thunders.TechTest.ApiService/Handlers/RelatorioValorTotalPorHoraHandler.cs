﻿using Rebus.Handlers;
using Thunders.TechTest.ApiService.Messages;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Services.Interfaces;

namespace Thunders.TechTest.ApiService.Jobs
{
    public class RelatorioValorTotalPorHoraHandler(IRelatorioService relatorioService) : IHandleMessages<RelatorioValorTotalPorHoraMessage>
    {
        public async Task Handle(RelatorioValorTotalPorHoraMessage message)
        {
            await relatorioService.GerarRelatorioValorTotalPorHora(message.Id, message.PeriodoInicial, message.PeriodoFinal);
        }
    }
}