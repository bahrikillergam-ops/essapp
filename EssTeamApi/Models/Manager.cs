namespace EssTeamApi.Models
{
    public class Manager
    {
        public int ManagerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Role { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
