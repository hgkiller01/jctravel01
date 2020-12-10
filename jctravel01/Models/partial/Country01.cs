using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace jctravel01.Models
{
    [MetadataType(typeof(Country01MD))]
    public partial class Country01
    {
        public class Country01MD 
        { 
            [ScaffoldColumn(true)]
            [DisplayName("國家代碼")]
            [StringLength(2,ErrorMessage="{0}不能超過{1}個字")]            
            [Required(ErrorMessage="{0}必填")]
            [RegularExpression(@"^[a-zA-Z]{2,2}$",ErrorMessage="不正確的代碼")]
            public string Country_no { get; set; }
            [DisplayName("索引值")]
            public int CountryIndex { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage="{0}必填")]
            [StringLength(20,ErrorMessage="{0}不能超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string ShortName { get; set; }     
            [DisplayName("中文名稱")]
            [Required(ErrorMessage="{0}必填")]
            [StringLength(30,ErrorMessage="{0}不能超過{0}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage="{0}必填")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            [StringLength(30,ErrorMessage="{0}不能超過{1}個字")]
            public string Ename { get; set; }
            [DisplayName("洲名")]
            [Required(ErrorMessage="{0}必填")]
            [StringLength(30,ErrorMessage="{0}不能超過{0}字")]
            public string Continent { get; set; }
            [DisplayName("線別")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public int PDivisionIndex { get; set; }
            [DisplayName("國碼")]
            [StringLength(6,ErrorMessage="{0}不能超過{1}")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_CountryCode { get; set; }
            [DisplayName("冠碼")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字.")]
            public Nullable<int> Tele_DialCode { get; set; }
            [DisplayName("電壓")]
            [StringLength(10,ErrorMessage="{0}不能超過{1}個字")]
            public string Voltage { get; set; }
            [DisplayName("周波數")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字.")]
            public Nullable<int> Frequency { get; set; }
            [DisplayName("適用插座代號")]
            [StringLength(2,ErrorMessage="{0}不能超過{1}字")]
            public string Plugcode { get; set; }
            [DisplayName("幣別代號")]
            [StringLength(6,ErrorMessage="{0}不能超過{1}")]
            [RegularExpression(@"^[a-zA-Z'''-'\s]{1,6}$", ErrorMessage = "不正確的代碼")]
            public string Currency_code { get; set; }
            [DisplayName("加值稅")]
            public Nullable<double> Tax { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1,3,ErrorMessage="{0}必需要在{1}到{2}之間")]
            public Nullable<int> Status { get; set; }
            [DisplayName("順序")]
            [Range(1,255,ErrorMessage="{0}必需要在{1}到{2}之間")]
            public int Order { get; set; }
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