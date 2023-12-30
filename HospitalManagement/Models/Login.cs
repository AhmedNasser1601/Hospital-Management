using FluentValidation;

namespace HospitalManagement.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Enter Email: ").EmailAddress().WithMessage("Enter Valid Email: ");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter Password: ").Length(6, 20);
        }
    }
}