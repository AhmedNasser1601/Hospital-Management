using FluentValidation;

namespace HospitalManagement.Models
{
    public class Register
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Enter Name: ").Length(2, 10);
            RuleFor(x => x.Email).NotEmpty().WithMessage("Enter Email: ").EmailAddress().WithMessage("Enter Valid Email: ");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Enter Password: ").Length(6, 20);
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("Enter Confirm Password: ").Equal(x => x.Password).WithMessage("Password and Confirm Password must be same");
        }
    }
}