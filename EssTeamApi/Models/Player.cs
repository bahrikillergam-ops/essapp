using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("players")]
    public class Player
    {
        [Key]
        [Column("player_id")]
        public int PlayerId { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("position")]
        public string? Position { get; set; }

        [Column("jersey_number")]
        public int JerseyNumber { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        // FK → managers
        [Column("manager_id")]
        public int ManagerId { get; set; }

        // Navigation
        public Manager? Manager { get; set; }

        public ICollection<PlayerTraining>? PlayerTrainings { get; set; }
    }
}
