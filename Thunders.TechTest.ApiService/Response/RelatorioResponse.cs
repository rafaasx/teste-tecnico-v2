using Thunders.TechTest.ApiService.Enums;

namespace Thunders.TechTest.ApiService.Response
{
    public record RelatorioResponse(Guid Id, StatusRelatorioEnum Status, object? Dados = null);
}
