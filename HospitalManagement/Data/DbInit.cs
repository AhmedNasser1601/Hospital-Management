using HospitalManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalManagement.Data
{
    public class DbInit
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var RoleM = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var UserM = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();

                if (!await RoleM.RoleExistsAsync(UserRole.Admin))
                    await RoleM.CreateAsync(new IdentityRole(UserRole.Admin));
                if (!await RoleM.RoleExistsAsync(UserRole.User))
                    await RoleM.CreateAsync(new IdentityRole(UserRole.User));

                if (await UserM.FindByEmailAsync("admin@gmail.com") == null)
                {
                    var AdminPerson = new User()
                    {
                        Name = "Admin",
                        UserName = "admin",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true
                    };

                    await UserM.CreateAsync(AdminPerson, "Admin*123");
                    await UserM.AddToRoleAsync(AdminPerson, UserRole.Admin);
                }

                if (await UserM.FindByEmailAsync("user@gmail.com") == null)
                {
                    var UserPerson = new User()
                    {
                        Name = "User",
                        UserName = "user",
                        Email = "user@gmail.com",
                        EmailConfirmed = true
                    };

                    await UserM.CreateAsync(UserPerson, "User*123");
                    await UserM.AddToRoleAsync(UserPerson, UserRole.Admin);
                }
            }
        }

        internal static object SeedUsersAndRolesAsync(IServiceProvider services)
        {
            throw new NotImplementedException();
        }
    }
}
