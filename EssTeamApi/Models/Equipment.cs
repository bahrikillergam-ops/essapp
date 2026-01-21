using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EssTeamApi.Models
{
    [Table("equipment")]
    public class Equipment
    {
        [Key]
        [Column("equipment_id")]
        public int EquipmentId { get; set; }

        [Column("equipment_name")]
        public string? EquipmentName { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("condition")]
        public string? Condition { get; set; }

        [Column("manager_id")]
        public int ManagerId { get; set; }

        public Manager? Manager { get; set; }
    }
}
