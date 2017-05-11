namespace PerformersUpdater.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Songs
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Songs()
        {
            Accords = new HashSet<Accord>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public int? PerformerId { get; set; }

        public int ViewsCount { get; set; }

        public int Number { get; set; }

        public virtual Performer Performers { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Accord> Accords { get; set; }
    }
}
