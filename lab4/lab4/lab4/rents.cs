//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace lab4
{
    using System;
    using System.Collections.Generic;
    
    public partial class rents
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rents()
        {
            this.bill_details = new HashSet<bill_details>();
        }
    
        public int rent_id { get; set; }
        public Nullable<int> apartment_id { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> date_begin { get; set; }
        public Nullable<System.DateTime> date_end { get; set; }
        public string client_passport_number { get; set; }
        public string type { get; set; }
    
        public virtual apartments apartments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bill_details> bill_details { get; set; }
        public virtual clients clients { get; set; }
    }
}
