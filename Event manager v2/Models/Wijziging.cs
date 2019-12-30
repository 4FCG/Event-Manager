namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wijziging")]
    public partial class Wijziging
    {
        [Key]
        public int wijziging_id { get; set; }

        public int beheerder { get; set; }

        public int type { get; set; }

        [Required]
        [StringLength(1000)]
        public string query { get; set; }

        [Required]
        [StringLength(100)]
        public string naam { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string beschrijving { get; set; }

        public virtual EvenementBeheerder EvenementBeheerder { get; set; }

        public virtual WijzigingsType WijzigingsType { get; set; }
    }
}
