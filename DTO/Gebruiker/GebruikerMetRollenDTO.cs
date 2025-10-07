namespace WebAPIDemo.DTO.Gebruiker
{
    public class GebruikerMetRollenDTO
    {
        public string Id { get; set; }

        [Display(Name = "Gebruiker")]
        public string UserName { get; set; }

        public string Email { get; set; }

        public IList<string> Roles { get; set; }
    }
}
