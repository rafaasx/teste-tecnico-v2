using FluentValidation;
using Thunders.TechTest.ApiService.Request;

namespace Thunders.TechTest.ApiService.Validators
{
    public class RelatorioRankingPracasPorMesRequestValidator : AbstractValidator<RelatorioRankingPracasPorMesRequest>
    {
        public RelatorioRankingPracasPorMesRequestValidator()
        {
            RuleFor(x => x.Ano)
                .NotEmpty()
                .GreaterThan(1900);
            RuleFor(x => x.Quantidade)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
