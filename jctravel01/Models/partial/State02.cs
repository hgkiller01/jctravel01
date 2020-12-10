using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(State02MD))]
    public partial class State02
    {
        public class State02MD
        {
            [ScaffoldColumn(true)]
            [DisplayName("州/省別代碼")]
            [Required(ErrorMessage="{0}為必填")]
            [StringLength(3,ErrorMessage="{0}字數不可超過{1}")]
            [RegularExpression(@"^[a-zA-Z]{3,3}$", ErrorMessage = "不正確的代碼")]
            public string State_no { get; set; }
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}為必填")]
            public int CountryIndex { get; set; }
            [DisplayName("州/省別索引")]
            [ScaffoldColumn(false)]
            public int StateIndex { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage="{0}為必填")]
            [StringLength(20,ErrorMessage="{0}字數不能超過{1}")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string ShortName { get; set; }
            [DisplayName("中文名稱")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30,ErrorMessage="{0}字數不能超過{1}")]
            public string Cname { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            [StringLength(20, ErrorMessage = "{0}字數不能超過{1}")]
            public string Ename { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1,3,ErrorMessage="{0}只能為{1}到{2}")]
            public Nullable<int> Status { get; set; }
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