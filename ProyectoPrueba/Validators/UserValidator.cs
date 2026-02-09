using FluentValidation;
using ProyectoPrueba.DTOs;

namespace ProyectoPrueba.Validators
{
    public class UserValidator : AbstractValidator<LoginDTO>
    {
        public UserValidator() 
        {
            RuleFor(x => x.Username).NotEmpty().Length(4,8);
            RuleFor(x => x.Password).NotEmpty().Length(4,8);
        }
    }
}
