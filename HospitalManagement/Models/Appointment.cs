using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HospitalManagement.Data;

namespace HospitalManagement.Models
{
    public class Appointment : IEntityBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PatientFName { get; set; }
        public string PatientLName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Disease { get; set; }
        public DateTime DateAndTime { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }

    public class AppointmentValidator : AbstractValidator<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(x =>x.PatientFName).NotEmpty().WithMessage("Enter First Name: ").Length(2, 10);
            RuleFor(x => x.PatientLName).NotEmpty().WithMessage("Enter Last Name: ").Length(2, 10);
            RuleFor(x => x.Age).NotEmpty().WithMessage("Enter Age: ");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Enter Phone Number: ");
            RuleFor(x => x.Disease).NotEmpty().WithMessage("Describe your Disease: ");
        }
    }

    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(x => x.PatientFName).IsRequired();
            builder.Property(x => x.PatientLName).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.Disease).IsRequired();
            builder.Property(x => x.DoctorId).IsRequired();
            builder.HasOne(x => x.Doctor).WithMany(x => x.Appointments).HasForeignKey(x => x.DoctorId);
        }
    }

    public interface IAppointmentService : IEntityBaseRepo<Appointment> { }
    public class AppointmentManager : EntityBaseRepository<Appointment>, IAppointmentService
    {
        public AppointmentManager(ApplicationDbContext context) : base(context) { }
    }
}
