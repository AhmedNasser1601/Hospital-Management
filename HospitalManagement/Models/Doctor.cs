using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;

namespace HospitalManagement.Models
{
    public class Doctor : IEntityBase
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Name => FName + " " + LName;
        public byte Age { get; set; }
        public byte Experience { get; set; }
        public Position Position { get; set; }
        public List<Appointment> Appointments { get; set; }
    }

    public class DoctorValidator : AbstractValidator<Doctor>
    {
        public DoctorValidator()
        {
            RuleFor(x => x.FName).NotEmpty().WithMessage("Enter First Name: ").Length(2, 10);
            RuleFor(x => x.LName).NotEmpty().WithMessage("Enter Last Name: ").Length(2, 10);
            RuleFor(x => x.Age).NotEmpty().WithMessage("Enter Age: ");
            RuleFor(x => x.Experience).NotEmpty().WithMessage("Write your Experience: ");
        }
    }

    public class DoctorConfiguration : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.FName).IsRequired();
            builder.Property(x => x.LName).IsRequired();
            builder.Property(x => x.Age).IsRequired();
        }
    }

    public enum Position
    {
        Surgeon,
        Neurologist,
        Gynecologist,
        Cardiologist,
        Endocrinologist,
        Pediatrician,
        Dentist,
        Oncologist,
        Radiologist,
        Urologist,
        Nephrologist
    }

    public interface IDoctorService : IEntityBaseRepo<Doctor> { }
    public class DoctorManager : EntityBaseRepository<Doctor>, IDoctorService
    {
        public DoctorManager(ApplicationDbContext context) : base(context) { }
    }
}
