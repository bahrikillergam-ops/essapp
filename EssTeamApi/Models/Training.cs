using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("trainings")]
    public class Training
    {
        [Key]
        [Column("training_id")]
        public int TrainingId { get; set; }

        [Column("training_date")]
        public DateTime TrainingDate { get; set; }

        [Column("time")]
        public TimeSpan Time { get; set; }

        [Column("location")]
        public string? Location { get; set; }

        [Column("focus")]
        public string? Focus { get; set; }

        [Column("manager_id")]
        public int ManagerId { get; set; }

        public Manager? Manager { get; set; }

        public ICollection<PlayerTraining>? PlayerTrainings { get; set; }
    }
}
