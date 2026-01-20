namespace EssTeamApi.Models
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string? Position { get; set; }
        public int? JerseyNumber { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
