using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("player_training")]
    public class PlayerTraining
    {
        [Column("player_id")]
        public int PlayerId { get; set; }
        public Player? Player { get; set; }

        [Column("training_id")]
        public int TrainingId { get; set; }
        public Training? Training { get; set; }
    }
}
