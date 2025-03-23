using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.ApiService.Request
{
    public record PedagioRequest(
              DateTime DataHora,
              string Praca,
              string Cidade,
              string Estado,
              decimal ValorPago,
              TipoVeiculoEnum TipoVeiculo
          )
    {

    }
}
