using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(DiningStyIndexMD))]
    public partial class DiningStyIndex
    {
        public class DiningStyIndexMD
        {
            public int DiningSty_Index { get; set; }
            [DisplayName("餐食方式代碼")]
            public string DiningSty_code { get; set; }
            [ScaffoldColumn(false)]
            [DisplayName("公司代號")]
            public string CompanyNo { get; set; }
            [DisplayName("方式簡稱")]
            [StringLength(20)]
            public string ShortName { get; set; }
            [DisplayName("中文名稱")]
            [StringLength(20)]
            [Required(ErrorMessage = "{0}必填")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("英文名稱")]
            [StringLength(50)]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string Ename { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
            [DisplayName("狀態")]
            [Required(ErrorMessage = "{0}必填")]
            public int Status { get; set; }
        }
    }
}