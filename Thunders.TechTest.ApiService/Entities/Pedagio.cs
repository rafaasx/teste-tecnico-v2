using Azure.Core;
using System.ComponentModel.DataAnnotations.Schema;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.ApiService.Entities
{
    [Table("Pedagios")]
    public class Pedagio
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public DateTime DataHora { get; private set; }
        public string Praca { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public decimal ValorPago { get; private set; }
        public TipoVeiculoEnum TipoVeiculo { get; private set; }

        public Pedagio(DateTime dataHora, string praca, string cidade, string estado, decimal valorPago, TipoVeiculoEnum tipoVeiculo)
        {
            DataHora = dataHora;
            Praca = praca;
            Cidade = cidade;
            Estado = estado;
            ValorPago = valorPago;
            TipoVeiculo = tipoVeiculo;
        }

        public List<string> ValidarDados()
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(Praca))
                erros.Add("Praça não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(Cidade))
                erros.Add("Cidade não pode ser vazia.");

            if (string.IsNullOrWhiteSpace(Estado))
                erros.Add("Estado não pode ser vazio.");

            if (ValorPago <= 0)
                erros.Add("Valor Pago deve ser maior que zero.");

            if (!Enum.IsDefined(typeof(TipoVeiculoEnum), TipoVeiculo))
                erros.Add("Tipo do veículo é inválido.");

            return erros;
        }
    }
}
