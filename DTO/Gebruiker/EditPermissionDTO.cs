namespace WebAPIDemo.DTO.Gebruiker
{
    public class EditPermissionDTO
    {
        public string Email { get; set; }

        public string RolNaam { get; set; }

        public EditOperation Operation { get; set; }
    }

    public enum EditOperation
    {
        Grant,
        Remove
    }
}
