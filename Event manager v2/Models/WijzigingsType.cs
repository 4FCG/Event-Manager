namespace Event_manager_v2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WijzigingsType")]
    public partial class WijzigingsType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WijzigingsType()
        {
            Wijzigings = new HashSet<Wijziging>();
        }

        [Key]
        public int type_id { get; set; }

        [Required]
        [StringLength(50)]
        public string naam { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Wijziging> Wijzigings { get; set; }
    }
}
