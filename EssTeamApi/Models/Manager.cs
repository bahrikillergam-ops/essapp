using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("managers")]
    public class Manager
    {
        [Key]
        [Column("manager_id")]
        public int ManagerId { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("role")]
        public string? Role { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        // Navigation
        public ICollection<Player>? Players { get; set; }
        public ICollection<Match>? Matches { get; set; }
        public ICollection<Training>? Trainings { get; set; }
        public ICollection<Equipment>? Equipment { get; set; }
    }
}
