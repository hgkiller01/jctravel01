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
    
    public partial class AirlineOffice
    {
        public int AirlineOfficeIndex { get; set; }
        public int CountryIndex { get; set; }
        public int CityIndex { get; set; }
        public int AirlineIndex { get; set; }
        public string ALoffice_Code { get; set; }
        public string Tele_Order { get; set; }
        public string Office_Number { get; set; }
        public string Office_Fax { get; set; }
        public string Office_Addr { get; set; }
        public string Office_Mailbox { get; set; }
        public int Status { get; set; }
        public int CreateBy { get; set; }
        public System.DateTime CreateBy_Time { get; set; }
        public int UpdateBy { get; set; }
        public System.DateTime UpdateBy_Time { get; set; }
        public string CompanyNo { get; set; }
    
        public virtual Airline Airline { get; set; }
        public virtual City03 City03 { get; set; }
        public virtual Country01 Country01 { get; set; }
    }
}
