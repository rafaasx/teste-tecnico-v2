using FluentValidation;
using Thunders.TechTest.ApiService.Enums;
using Thunders.TechTest.ApiService.Request;

namespace Thunders.TechTest.ApiService.Validators
{
    public class PedagioMessageValidator : AbstractValidator<PedagioRequest>
    {
        public PedagioMessageValidator()
        {
            RuleFor(x => x.DataHora).NotEmpty();
            RuleFor(x => x.Praca).NotEmpty();
            RuleFor(x => x.Cidade).NotEmpty();
            RuleFor(x => x.Estado)
                .NotEmpty()
                .Length(2).WithMessage("O campo Estado deve conter 2 caracteres.");
            RuleFor(x => x.ValorPago)
                .GreaterThan(0).WithMessage("O valor pago deve ser maior que zero.");
            RuleFor(x => x.TipoVeiculo)
                .Must(v => Enum.IsDefined(typeof(TipoVeiculoEnum), v))
                .WithMessage("Tipo de veículo inválido.");
        }
    }
}
