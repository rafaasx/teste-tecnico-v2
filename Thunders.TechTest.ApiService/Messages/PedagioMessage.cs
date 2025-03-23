using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.ApiService.Messages
{
    public record PedagioMessage(
           DateTime DataHora,
           string Praca,
           string Cidade,
           string Estado,
           decimal ValorPago,
           TipoVeiculoEnum TipoVeiculo,
           Guid Id
       )
    {
    }
}
