using Moq;
using Newtonsoft.Json;
using Thunders.TechTest.ApiService.Entities;
using Thunders.TechTest.ApiService.Enums;
using Thunders.TechTest.ApiService.Factory;
using Thunders.TechTest.ApiService.Persistence.Interfaces;
using Thunders.TechTest.ApiService.Repositories.Interfaces;
using Thunders.TechTest.ApiService.Response;
using Thunders.TechTest.ApiService.Services.Implementations;
using Thunders.TechTest.OutOfBox.Queues;

namespace Thunders.TechTest.Tests.Services
{
    public class RelatorioServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMessageSender> _messageSernderMock;

        private readonly Mock<IPedagioRepository> _pedagioRepositoryMock;
        private readonly Mock<IRelatorioRepository> _relatorioRepositoryMock;
        private readonly RelatorioService _relatorioService;

        public RelatorioServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _pedagioRepositoryMock = new Mock<IPedagioRepository>();
            _relatorioRepositoryMock = new Mock<IRelatorioRepository>();
            _messageSernderMock = new Mock<IMessageSender>();

            _unitOfWorkMock.Setup(u => u.PedagioRepository).Returns(_pedagioRepositoryMock.Object);
            _unitOfWorkMock.Setup(u => u.RelatorioRepository).Returns(_relatorioRepositoryMock.Object);

            _relatorioService = new RelatorioService(_unitOfWorkMock.Object, _messageSernderMock.Object);
        }

        [Fact]
        public async Task GerarRelatorioValorTotalPorHora_Gerar_Relatorio_Correto_5_Horas_Distintas_Somando_Valor_Total()
        {
            string cidade = "Joinville";
            var ano = 2025;
            var periodoInicial = new DateTime(ano, 1, 10);
            var periodoFinal = new DateTime(ano, 1, 10);
            var relatorio = RelatorioFactory.CriarRelatorioValorTotalPorHora(periodoInicial, periodoFinal);
            var pedagios = new List<Pedagio>
            {
                new(new DateTime(ano, 1, 10, 01, 20 , 30), "Praça A", cidade, "SC", 10, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 02, 20 , 30), "Praça A", cidade, "SC", 20, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 03, 20 , 30), "Praça A", cidade, "SC", 30, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 04, 20 , 30), "Praça A", cidade, "SC", 40, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 05, 20 , 30), "Praça A", cidade, "SC", 50, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 06, 20 , 30), "Praça A", cidade, "SC", 60, TipoVeiculoEnum.Carro),
                new(new DateTime(ano, 1, 10, 07, 20 , 30), "Praça A", cidade, "SC", 70, TipoVeiculoEnum.Carro),
            };
            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagios);
            _relatorioRepositoryMock.Setup(repo => repo.Query()).Returns(new List<Relatorio>() { relatorio }.AsQueryable());
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);

            await _relatorioService.GerarRelatorioValorTotalPorHora(relatorio.Id, periodoInicial, periodoFinal);

            Assert.NotNull(relatorio.Dados);
            var dadosProcessados = JsonConvert.DeserializeObject<List<RelatorioValorTotalPorHoraResponse>>(relatorio.Dados);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.NotNull(dadosProcessados);
            Assert.Equal(pedagios.Select(s => s.DataHora.Hour).Distinct().Count(), dadosProcessados.Count);
            Assert.Contains(dadosProcessados, r => r.Hora == 1 && r.ValorTotal == 10);
            Assert.Contains(dadosProcessados, r => r.Hora == 2 && r.ValorTotal == 20);
            Assert.Contains(dadosProcessados, r => r.Hora == 3 && r.ValorTotal == 30);
            Assert.Contains(dadosProcessados, r => r.Hora == 4 && r.ValorTotal == 40);
            Assert.Contains(dadosProcessados, r => r.Hora == 5 && r.ValorTotal == 50);
            Assert.Contains(dadosProcessados, r => r.Hora == 6 && r.ValorTotal == 60);
            Assert.Contains(dadosProcessados, r => r.Hora == 7 && r.ValorTotal == 70);
        }

        [Fact]
        public async Task GerarRelatorioValorTotalPorHora_Nao_Deve_Gerar_O_Relatorio()
        {
            string cidade = "CIDADE_INVALIDA";
            var periodoInicial = DateTime.UtcNow;
            var periodoFinal = DateTime.UtcNow;
            var pedagioData = new List<Pedagio>();
            var relatorio = RelatorioFactory.CriarRelatorioValorTotalPorHora(periodoInicial, periodoFinal);

            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagioData);
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);

            await _relatorioService.GerarRelatorioValorTotalPorHora(relatorio.Id, periodoInicial, periodoFinal);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task GerarRelatorioRankingPracasPorMes_Deve_Gerar_Relatorio_Ordenado_Limitado_O_Resultado()
        {
            int limiteResultadosPorMes = 2;
            var ano = DateTime.Now.Year;
            string cidade = "Joinville";

            var relatorio = RelatorioFactory.CriarRelatorioRankingPracasPorMes(ano, limiteResultadosPorMes);

            var pedagios = new List<Pedagio>
            {
                    // Janeiro
                    new(new DateTime(ano, 1, 10), "Praça A", cidade, "SC", 15, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 1, 15), "Praça B", cidade, "SC", 25, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 1, 20), "Praça C", cidade, "SC", 35, TipoVeiculoEnum.Carro),

                    // Março
                    new(new DateTime(ano, 3, 5), "Praça A", cidade, "SC", 20, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 3, 12), "Praça B", cidade, "SC", 30, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 3, 22), "Praça C", cidade, "SC", 40, TipoVeiculoEnum.Carro),

                    // Maio
                    new(new DateTime(ano, 5, 8), "Praça A", cidade, "SC", 25, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 5, 18), "Praça B", cidade, "SC", 35, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 5, 28), "Praça C", cidade, "SC", 45, TipoVeiculoEnum.Carro),

                    // Julho
                    new(new DateTime(ano, 7, 3), "Praça A", cidade, "SC", 30, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 7, 14), "Praça B", cidade, "SC", 40, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 7, 25), "Praça C", cidade, "SC", 50, TipoVeiculoEnum.Carro),

                    // Setembro
                    new(new DateTime(ano, 9, 7), "Praça A", cidade, "SC", 35, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 9, 17), "Praça B", cidade, "SC", 45, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 9, 27), "Praça C", cidade, "SC", 55, TipoVeiculoEnum.Carro),

                    // Novembro
                    new(new DateTime(ano, 11, 2), "Praça A", cidade, "SC", 40, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 11, 16), "Praça B", cidade, "SC", 50, TipoVeiculoEnum.Carro),
                    new(new DateTime(ano, 11, 29), "Praça C", cidade, "SC", 60, TipoVeiculoEnum.Carro),
                };

            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagios);
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);
            _relatorioRepositoryMock.Setup(repo => repo.Query()).Returns(new List<Relatorio>() { relatorio }.AsQueryable());

            await _relatorioService.GerarRelatorioRankingPracasPorMes(relatorio.Id, ano, limiteResultadosPorMes);

            Assert.NotNull(relatorio.Dados);
            var dadosProcessados = JsonConvert.DeserializeObject<List<RelatorioRankingPracasPorMesResponse>>(relatorio.Dados);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.NotNull(dadosProcessados);

            var mesesNoRelatorio = dadosProcessados.Select(d => d.Mes).Distinct().ToList();
            Assert.Equal(6, mesesNoRelatorio.Count);

            var agrupadosPorMes = dadosProcessados.GroupBy(d => new { d.Ano, d.Mes });
            foreach (var grupo in agrupadosPorMes)
                Assert.True(grupo.Count() <= limiteResultadosPorMes, $"O periodo {grupo.Key.Ano}/{grupo.Key.Mes} tem mais de {limiteResultadosPorMes} registros.");

            foreach (var grupo in agrupadosPorMes)
            {
                var valoresOrdenados = grupo.OrderByDescending(d => d.TotalFaturado).ToList();
                Assert.Equal(valoresOrdenados, grupo.ToList());
            }
        }

        [Fact]
        public async Task GerarRelatorioRankingPracasPorMes_Nao_Deve_Gerar_O_Relatorio()
        {
            int limiteResultadosPorMes = 2;
            var ano = DateTime.Now.Year;
            string cidade = "Joinville";

            var relatorio = RelatorioFactory.CriarRelatorioRankingPracasPorMes(ano, limiteResultadosPorMes);

            var pedagios = new List<Pedagio>();

            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagios);
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);
            _relatorioRepositoryMock.Setup(repo => repo.Query()).Returns(new List<Relatorio>() { relatorio }.AsQueryable());

            await _relatorioService.GerarRelatorioRankingPracasPorMes(relatorio.Id, ano, limiteResultadosPorMes);

            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Never);
        }


        [Fact]
        public async Task GerarRelatorioVeiculosPorPraca_Deve_Gerar_Relatorio_Correto()
        {
            string praca = "Praça A";
            var cidade = "Joinville";
            var relatorio = RelatorioFactory.CriarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(praca);

            var pedagios = new List<Pedagio>
            {
                new(DateTime.Now, "Praça A", cidade, "SC", 10, TipoVeiculoEnum.Carro),
                new(DateTime.Now, "Praça A", cidade, "SC", 10, TipoVeiculoEnum.Moto),
                new(DateTime.Now, "Praça A", cidade, "SC", 50, TipoVeiculoEnum.Caminhao),
                new(DateTime.Now, "Praça A", cidade, "SC", 50, TipoVeiculoEnum.Carro),
            };

            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagios.AsQueryable());
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);
            _relatorioRepositoryMock.Setup(repo => repo.Query()).Returns(new List<Relatorio>() { relatorio }.AsQueryable());

            await _relatorioService.GerarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(relatorio.Id, praca);

            Assert.NotNull(relatorio.Dados);
            var dadosProcessados = JsonConvert.DeserializeObject<RelatorioQuantidadeDeTiposDeVeiculosPorPracaResponse>(relatorio.Dados);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.NotNull(dadosProcessados);
            Assert.Equal(3, dadosProcessados.Quantidade);
        }

        [Fact]
        public async Task GerarRelatorioVeiculosPorPraca_Deve_Gerar_O_Relatorio_Igual_A_Zero()
        {
            string praca = "Praça A";
            var relatorio = RelatorioFactory.CriarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(praca);
            var pedagios = new List<Pedagio>();
            var mockDbSet = DbSetMockHelper.CreateDbSetMock(pedagios.AsQueryable());
            _pedagioRepositoryMock.Setup(repo => repo.Query()).Returns(mockDbSet.Object);
            _relatorioRepositoryMock.Setup(repo => repo.Query()).Returns(new List<Relatorio>() { relatorio }.AsQueryable());

            await _relatorioService.GerarRelatorioQuantidadeDeTiposDeVeiculosPorPraca(relatorio.Id, praca);

            Assert.NotNull(relatorio.Dados);
            var dadosProcessados = JsonConvert.DeserializeObject<RelatorioQuantidadeDeTiposDeVeiculosPorPracaResponse>(relatorio.Dados);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            Assert.NotNull(dadosProcessados);
            Assert.Equal(0, dadosProcessados.Quantidade);

        }
    }
}
