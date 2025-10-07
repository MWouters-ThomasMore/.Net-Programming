using Microsoft.AspNetCore.Identity;
using System.Data.Common;
using WebAPIDemo.Data.Resources;

namespace WebAPIDemo.Data
{
    public class IdentitySeeding
    {
        public async Task IdentitySeedingAsync(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            try
            {
                // Rollen aanmaken
                // User Role
                bool roleExists = await roleManager.RoleExistsAsync(StringResources.Role_User);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(StringResources.Role_User));
                }

                // Admin Role
                roleExists = await roleManager.RoleExistsAsync(StringResources.Role_Admin);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(StringResources.Role_Admin));
                }

                // Gebruiker aanmaken
                // Admin bestaat nog niet?
                if (userManager.FindByNameAsync(StringResources.Role_Admin).Result == null)
                {
                    // Gebruiker voorzien
                    var defaultUser = new CustomUser
                    {
                        UserName = StringResources.Role_Admin,
                        Email = "superteam@thomasmore.be",
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true
                    };

                    // Gebruiker aanmaken met Role
                    IdentityResult admin = await userManager.CreateAsync(defaultUser, "t0LTHxzy.v");

                    if (admin.Succeeded 
                        && userManager.FindByNameAsync(StringResources.Role_Admin).Result != null)
                    {
                        // Voeg gebruiker toe aan Role
                        await userManager.AddToRoleAsync(defaultUser, StringResources.Role_Admin);
                    }
                }
            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
