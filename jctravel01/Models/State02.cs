//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace jctravel01.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class State02
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public State02()
        {
            this.City03 = new HashSet<City03>();
            this.TourBure = new HashSet<TourBure>();
        }
    
        public int StateIndex { get; set; }
        public int CountryIndex { get; set; }
        public string State_no { get; set; }
        public string ShortName { get; set; }
        public string Cname { get; set; }
        public string Ename { get; set; }
        public Nullable<int> Status { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
        public string CompanyNo { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<City03> City03 { get; set; }
        public virtual Country01 Country01 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourBure> TourBure { get; set; }
    }
}