//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TeacherManager.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class TIME_SLOT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TIME_SLOT()
        {
            this.ARRANGE_TIME_SLOT = new HashSet<ARRANGE_TIME_SLOT>();
        }
    
        public int ID { get; set; }
        public Nullable<int> ID_DAY { get; set; }
        public string NAME { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ARRANGE_TIME_SLOT> ARRANGE_TIME_SLOT { get; set; }
        public virtual DAY DAY { get; set; }
    }
}