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
    
    public partial class CoIndex
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CoIndex()
        {
            this.HRInfo = new HashSet<HRInfo>();
            this.PasMng = new HashSet<PasMng>();
            this.PermiGruop = new HashSet<PermiGruop>();
            this.PermiMng = new HashSet<PermiMng>();
        }
    
        public string CompanyNo { get; set; }
        public string ComName { get; set; }
        public string TaxID { get; set; }
        public string ContactPhone { get; set; }
        public string ContactName { get; set; }
        public string Co_addres { get; set; }
        public string CoIP { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
        public bool Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HRInfo> HRInfo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PasMng> PasMng { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PermiGruop> PermiGruop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PermiMng> PermiMng { get; set; }
    }
}