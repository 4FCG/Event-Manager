namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Activiteit")]
    public partial class Activiteit
    {
        [Key]
        public int activiteit_id { get; set; }

        [Required]
        [StringLength(100)]
        public string naam { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string beschrijving { get; set; }

        public int evenement { get; set; }

        public DateTime begintijd { get; set; }

        public DateTime eindtijd { get; set; }

        public int evenement_beheerder { get; set; }

        public virtual Evenement Evenement1 { get; set; }

        public virtual EvenementBeheerder EvenementBeheerder { get; set; }
    }
}
