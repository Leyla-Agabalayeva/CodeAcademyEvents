using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAcademyEvents.DAL.Data
{
    /// <summary>
    /// Tətbiq ilk dəfə işə düşəndə "Admin" rolunu və 1 default admin hesabını yaradır.
    /// Heç bir biznes data (Event, Location və s.) burada YARADILMIR —
    /// bunları admin öz hesabı ilə daxil olub sayt üzərindən əlavə edir.
    /// Program.cs-də app build olunandan sonra çağırılır: await DbInitializer.SeedAdminAsync(app.Services);
    /// </summary>
    public static class DbInitializer
    {
        public const string AdminRole = "Admin";
        public const string AdminEmail = "admin@codeacademy.az";
        public const string AdminPassword = "Admin123!"; // İlk girişdən sonra dəyişdirilməlidir

        public static async Task SeedAdminAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1) "Admin" rolunu yarat (yoxdursa)
            if (!await roleManager.RoleExistsAsync(AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(AdminRole));
            }

            // 2) Default admin istifadəçisini yarat (yoxdursa)
            var adminUser = await userManager.FindByEmailAsync(AdminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = AdminEmail,
                    Email = AdminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, AdminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, AdminRole);
                }
            }
            else if (!await userManager.IsInRoleAsync(adminUser, AdminRole))
            {
                await userManager.AddToRoleAsync(adminUser, AdminRole);
            }
        }
    }

}
