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
    
    public partial class CityDistrict
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CityDistrict()
        {
            this.Hotel = new HashSet<Hotel>();
            this.Restaurant = new HashSet<Restaurant>();
            this.Scenery = new HashSet<Scenery>();
        }
    
        public int CityDistrictIndex { get; set; }
        public int CityIndex { get; set; }
        public string CityDistrictCode { get; set; }
        public string DisCname { get; set; }
        public string DisEname { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
        public string CompanyNo { get; set; }
    
        public virtual City03 City03 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Hotel> Hotel { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Restaurant> Restaurant { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Scenery> Scenery { get; set; }
    }
}
