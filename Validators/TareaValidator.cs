using FluentValidation;                    // ← ESTE ES CRÍTICO
using TareaAPI.Models.DTOs;

namespace TareaAPI.Validators;

public class TareaValidator : AbstractValidator<TareaDto>
{
    public TareaValidator()
    {
        RuleFor(t => t.Nombre)
            .NotEmpty().WithMessage("El nombre de la tarea es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre de la tarea no puede exceder 200 caracteres.");
    }
}

public class TareaUpdateValidator : AbstractValidator<TareaUpdateDto>
{
    public TareaUpdateValidator()
    {
        When(t => t.Nombre != null, () =>
        {
            RuleFor(t => t.Nombre)
                .MaximumLength(200)
                .WithMessage("El nombre de la tarea no puede exceder 200 caracteres.");
        });
    }
}
