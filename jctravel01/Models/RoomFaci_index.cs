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
    
    public partial class RoomFaci_index
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomFaci_index()
        {
            this.RoomFacility = new HashSet<RoomFacility>();
        }
    
        public int RoomFaci_no { get; set; }
        public string RoomFaci_code { get; set; }
        public string CompanyNo { get; set; }
        public string ShortName { get; set; }
        public string Cname { get; set; }
        public string Ename { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
        public int Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomFacility> RoomFacility { get; set; }
    }
}