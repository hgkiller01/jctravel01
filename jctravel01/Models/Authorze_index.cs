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
    
    public partial class Authorze_index
    {
        public int AutIndex { get; set; }
        public int PermiIndex { get; set; }
        public int permiGpIndex { get; set; }
        public bool ReadPermi { get; set; }
        public bool ChPermi { get; set; }
        public bool SharePermi { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
    
        public virtual PermiGruop PermiGruop { get; set; }
        public virtual PermiIndex PermiIndex1 { get; set; }
    }
}
