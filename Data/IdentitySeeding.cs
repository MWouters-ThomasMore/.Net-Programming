using Microsoft.AspNetCore.Identity;
using System.Data.Common;

namespace WebAPIDemo.Data
{
    public class IdentitySeeding
    {
        public async Task IdentitySeedingAsync(UserManager<CustomUser> userManager)
        {

            try
            {
                // Gebruiker aanmaken
                // Admin bestaat nog niet?
                if (userManager.FindByNameAsync("Admin").Result == null)
                {
                    // Gebruiker voorzien
                    var defaultUser = new CustomUser
                    {
                        UserName = "Admin",
                        Email = "superteam@thomasmore.be",
                        EmailConfirmed = true
                    };

                    // Gebruiker aanmaken
                    await userManager.CreateAsync(defaultUser, "t0LTHxzy.v");

                }
            }
            catch (DbException ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
    }
}
