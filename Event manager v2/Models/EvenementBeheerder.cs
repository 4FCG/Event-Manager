namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EvenementBeheerder")]
    public partial class EvenementBeheerder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EvenementBeheerder()
        {
            Activiteits = new HashSet<Activiteit>();
            Wijzigings = new HashSet<Wijziging>();
        }

        public int evenement { get; set; }

        public int beheerder { get; set; }

        [Key]
        public int evenement_beheerder_id { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activiteit> Activiteits { get; set; }

        public virtual Beheerder Beheerder1 { get; set; }

        public virtual Evenement Evenement1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wijziging> Wijzigings { get; set; }
    }
}
