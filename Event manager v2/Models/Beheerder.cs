namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Beheerder")]
    public partial class Beheerder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Beheerder()
        {
            EvenementBeheerders = new HashSet<EvenementBeheerder>();
        }

        [Key]
        public int beheerder_id { get; set; }

        [Required]
        [StringLength(50)]
        public string voornaam { get; set; }

        [Required]
        [StringLength(50)]
        public string achternaam { get; set; }

        [Required]
        [StringLength(50)]
        public string gebruikersnaam { get; set; }

        [Required]
        [StringLength(100)]
        public string wachtwoord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EvenementBeheerder> EvenementBeheerders { get; set; }
    }
}
