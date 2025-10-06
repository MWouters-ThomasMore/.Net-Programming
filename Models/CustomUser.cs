using Microsoft.AspNetCore.Identity;

namespace WebAPIDemo.Models
{
    public class CustomUser : IdentityUser
    {
        [PersonalData]
        public string? Naam { get; set; }
        [PersonalData]
        public DateTime? Geboortedatum { get; set; }
    }
}
