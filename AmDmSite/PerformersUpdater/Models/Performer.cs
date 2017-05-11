namespace PerformersUpdater.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Performer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Performer()
        {
            Songs = new HashSet<Songs>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Biography { get; set; }

        public string PathToPhoto { get; set; }

        public int ViewsCount { get; set; }

        public int? SongsCount { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Songs> Songs { get; set; }
    }
}
