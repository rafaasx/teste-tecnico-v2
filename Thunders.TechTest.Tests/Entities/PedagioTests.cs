using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.Tests.Entities
{
    public class PedagioTests
    {
        public static IEnumerable<object[]> DadosInvalidos =>
       new List<object[]>
       {
            // Cenário 1: Praça vazia
            new object[] { new Pedagio(DateTime.Now, "", "Cidade A", "SP", 10, TipoVeiculoEnum.Carro), "Praça não pode ser vazia." },
            
            // Cenário 2: Cidade vazia
            new object[] { new Pedagio(DateTime.Now, "Praça A", "", "SP", 10, TipoVeiculoEnum.Carro), "Cidade não pode ser vazia." },

            // Cenário 3: Estado vazio
            new object[] { new Pedagio(DateTime.Now, "Praça A", "Cidade A", "", 10, TipoVeiculoEnum.Carro), "Estado não pode ser vazio." },

            // Cenário 4: Valor pago menor ou igual a zero
            new object[] { new Pedagio(DateTime.Now, "Praça A", "Cidade A", "SP", 0, TipoVeiculoEnum.Carro), "Valor Pago deve ser maior que zero." },

            // Cenário 5: Tipo de veículo inválido
            new object[] { new Pedagio(DateTime.Now, "Praça A", "Cidade A", "SP", 10, (TipoVeiculoEnum)999), "Tipo do veículo é inválido." }
       };

        [Theory]
        [MemberData(nameof(DadosInvalidos))]
        public void ValidarDados_DeveRetornarErrosParaDadosInvalidos(Pedagio pedagio, string mensagemEsperada)
        {
            var erros = pedagio.ValidarDados();

            Assert.Contains(mensagemEsperada, erros);
        }

        [Fact]
        public void ValidarDados_DeveRetornarListaVaziaParaDadosValidos()
        {
            var pedagio = new Pedagio(DateTime.Now, "Praça A", "Cidade A", "SP", 15.50m, TipoVeiculoEnum.Carro);

            var erros = pedagio.ValidarDados();

            Assert.Empty(erros);
        }
    }
}
