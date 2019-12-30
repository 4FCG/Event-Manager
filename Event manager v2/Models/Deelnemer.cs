namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Deelnemer")]
    public partial class Deelnemer
    {
        [Key]
        public int deelnemer_id { get; set; }

        [Required]
        [StringLength(50)]
        public string voornaam { get; set; }

        [Required]
        [StringLength(50)]
        public string achternaam { get; set; }

        [Required]
        [StringLength(100)]
        public string email { get; set; }

        public int evenement { get; set; }

        public byte goedgekeurd { get; set; }

        public virtual Evenement Evenement1 { get; set; }

    }
}
