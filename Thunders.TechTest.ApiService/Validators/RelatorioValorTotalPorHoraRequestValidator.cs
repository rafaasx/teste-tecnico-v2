using FluentValidation;
using Thunders.TechTest.ApiService.Request;

namespace Thunders.TechTest.ApiService.Validators
{
    public class RelatorioValorTotalPorHoraRequestValidator : AbstractValidator<RelatorioValorTotalPorHoraRequest>
    {
        public RelatorioValorTotalPorHoraRequestValidator()
        {
            RuleFor(x => x.PeriodoInicial)
                .NotEmpty();

            RuleFor(x => x.PeriodoFinal)
                .NotEmpty()
                .Must((request, periodoFinal) =>
                    periodoFinal.Date >= request.PeriodoInicial.Date)
                .WithMessage("A data final deve ser maior que a data inicial.");
        }
    }
}
