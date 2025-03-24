using FluentValidation;
using Thunders.TechTest.ApiService.Request;

namespace Thunders.TechTest.ApiService.Validators
{
    public class RelatorioQuantidadeDeTiposDeVeiculosPorPracaRequestValidator : AbstractValidator<RelatorioQuantidadeDeTiposDeVeiculosPorPracaRequest>
    {
        public RelatorioQuantidadeDeTiposDeVeiculosPorPracaRequestValidator()
        {
            RuleFor(x => x.Praca).NotEmpty();
        }
    }
}
