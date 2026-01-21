using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("matches")]
    public class Match
    {
        [Key]
        [Column("match_id")]
        public int MatchId { get; set; }

        [Column("match_date")]
        public DateTime MatchDate { get; set; }

        [Column("opponent")]
        public string? Opponent { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("result")]
        public string? Result { get; set; }

        [Column("manager_id")]
        public int ManagerId { get; set; }

        public Manager? Manager { get; set; }
    }
}
