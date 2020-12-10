using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(AirlineOfficeMD))]
    public partial class AirlineOffice
    {
        public class AirlineOfficeMD
        {
            [DisplayName("辦事處代碼")]
            [StringLength(9)]
            public string ALoffice_Code { get; set; }
            [DisplayName("航空公司代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int AirlineIndex { get; set; }
            [DisplayName("索引值")]
            public int AirlineOfficeIndex { get; set; }
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CountryIndex { get; set; }
            [DisplayName("城市代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CityIndex { get; set; }
            [DisplayName("訂位電話")]
            [StringLength(20)]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Tele_Order { get; set; }
            [DisplayName("電話")]
            [StringLength(20)]
            [Required(ErrorMessage = "{0}必填")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Office_Number { get; set; }
            [DisplayName("傳真")]
            [StringLength(20)]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Office_Fax { get; set; }
            [DisplayName("地址")]
            [Required(ErrorMessage = "{0}必填")]
            [DataType(DataType.MultilineText)]
            public string Office_Addr { get; set; }
            [DisplayName("郵政信箱")]
            [DataType(DataType.MultilineText)]
            public string Office_Mailbox { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1,3)]
            public int Status { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
            [DisplayName("公司編號")]
            [DefaultValue("00001")]
            public string CompanyNo { get; set; }
        }
    }
}