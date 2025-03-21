using System.ComponentModel.DataAnnotations.Schema;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.ApiService.Entities
{
    [Table("Relatorios")]
    public class Relatorio
    {
        public Relatorio(TipoRelatorioEnum tipoRelatorio, string parametros, string? dados = null)
        {
            TipoRelatorio = tipoRelatorio;
            Parametros = parametros;
            Dados = dados;
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public TipoRelatorioEnum TipoRelatorio { get; private set; }
        public DateTime DataProcessamento { get; private set; } = DateTime.UtcNow;
        public string Parametros { get; private set; }
        public string? Dados { get; private set; }
        public StatusRelatorioEnum Status { get; set; } = StatusRelatorioEnum.Pendente;
        public void ChangeDados(string dados) => Dados = dados;
        public void Processar() => Status = StatusRelatorioEnum.Processado;
        public void ProcessarComErro() => Status = StatusRelatorioEnum.Erro;
    }
}
