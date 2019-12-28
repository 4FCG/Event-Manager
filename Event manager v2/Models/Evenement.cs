namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Evenement")]
    public partial class Evenement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Evenement()
        {
            Activiteits = new HashSet<Activiteit>();
            Deelnemers = new HashSet<Deelnemer>();
            EvenementBeheerders = new HashSet<EvenementBeheerder>();
        }

        [Key]
        public int evenement_id { get; set; }

        [Required]
        [StringLength(100)]
        public string naam { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string beschrijving { get; set; }

        [Column(TypeName = "date")]
        public DateTime begindatum { get; set; }

        [Column(TypeName = "date")]
        public DateTime einddatum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activiteit> Activiteits { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deelnemer> Deelnemers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvenementBeheerder> EvenementBeheerders { get; set; }
    }
}
