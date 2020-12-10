using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(CityDistrictMD))]
    public partial class CityDistrict
    {
        public class CityDistrictMD
        {
        public int CityDistrictIndex { get; set; }
        [DisplayName("城市代碼")]
        public int CityIndex { get; set; }
        [DisplayName("區域代碼")]
        public string CityDistrictCode { get; set; }
        [DisplayName("中文名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(20, ErrorMessage = "{0}不能超過{0}個字")]
        [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
        public string DisCname { get; set; }
        [DisplayName("英文名稱")]
        [Required(ErrorMessage = "{0}必填")]
        [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
        [StringLength(30, ErrorMessage = "{0}不能超過{1}個字")]
        public string DisEname { get; set; }
        [DisplayName("建檔狀態")]
        [Required(ErrorMessage = "{0}必填")]
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
        public string CompanyNo { get; set; }
        }
       
    }
}