using FluentValidation;
using ProyectoPrueba.DTOs;

namespace ProyectoPrueba.Validators
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            // Regla para el Nombre: No vacío, longitud mínima y máxima
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .Length(3, 100).WithMessage("El nombre debe tener entre 3 y 100 caracteres.");

            // Regla para el Precio: Mayor a 0
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
                

            // Regla para el Stock: No puede ser negativo
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

            // Regla para Descripción: Opcional pero con límite si se envía
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
        }
    }
}
