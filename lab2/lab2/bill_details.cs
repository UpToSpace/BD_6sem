//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace lab2
{
    using System;
    using System.Collections.Generic;
    
    public partial class bill_details
    {
        public int bill_id { get; set; }
        public Nullable<int> rent_id { get; set; }
        public Nullable<System.DateTime> bill_date { get; set; }
        public Nullable<decimal> total { get; set; }
    
        public virtual rent rent { get; set; }
    }
}
