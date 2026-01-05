using FluentValidation;
using TareaAPI.Models.DTOs;

namespace TareaAPI.Validators
{
    public class CategoriaValidator: AbstractValidator<CategoriaCreateDto>
    {
        public CategoriaValidator()
        {
            RuleFor(c => c.Nombre)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre de la categoría no puede exceder 100 caracteres.");

            RuleFor(c => c.Descripcion)
                .MaximumLength(200).WithMessage("La descripción de la categoría no puede exceder 200 caracteres.");
        }
        
    }

    public class CategoriaUpdateValidator : AbstractValidator<CategoriaUpdateDto>
    {
        public CategoriaUpdateValidator()
        {
            When(c => c.Nombre != null, () =>
            {
                RuleFor(c => c.Nombre)
                    .MaximumLength(200)
                    .WithMessage("El nombre de la categoría no puede exceder 200 caracteres.");
            });
        }
    }
}