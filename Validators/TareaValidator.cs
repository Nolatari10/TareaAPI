using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TareaAPI.Data;
using TareaAPI.Models.DTOs;                    // ← ESTE ES CRÍTICO

namespace TareaAPI.Validators;

public class TareaValidator : AbstractValidator<TareaCreateDto>
{
    private readonly AppDbContext _context;
    public TareaValidator(AppDbContext context)
    {
        _context = context;

        RuleFor(t => t.Nombre)
            .NotEmpty().WithMessage("El nombre de la tarea es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre de la tarea no puede exceder 200 caracteres.");
  
        RuleFor(t => t.CategoriaId)
            .MustAsync(async (id, cancellation) =>
            {
                if(id == null) return true; 
                // Here you would typically inject a repository or DbContext to check if the category exists.
                // For demonstration purposes, let's assume a method CategoryExistsAsync(id) exists.
                return await _context.Categorias.AnyAsync(c => c.categoriaId == id, cancellation);
            }).WithMessage("La categoría especificada no existe.");
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
