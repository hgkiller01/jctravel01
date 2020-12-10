using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(AirportInfoMD))]
    public partial class AirportInfo
    {
        public class AirportInfoMD
        {
            [ScaffoldColumn(true)]
            [DisplayName("機場代碼")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(3,ErrorMessage="只能為3個字",MinimumLength=3)]
            [RegularExpression(@"^[a-zA-Z]{3,3}$", ErrorMessage = "不正確的代碼")]
            public string AirportCode { get; set; }
            [DisplayName("索引值")]
            [ScaffoldColumn(false)]
            public int AirportIndex { get; set; }
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}為必填")]
            public int CountryIndex { get; set; }
            [DisplayName("城市代碼")]
            [Required(ErrorMessage = "{0}為必填")]
            public int CityIndex { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(50, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string ApEName { get; set; }
            [DisplayName("中文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string ApCname { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1, 3)]
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