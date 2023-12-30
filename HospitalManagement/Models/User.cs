using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagement.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) { }
    }

    public class UserRole
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}
