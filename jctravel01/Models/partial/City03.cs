using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(City03MD))]
    public partial class City03
    {
        public class City03MD
        {
            [ScaffoldColumn(true)]
            [DisplayName("城市代碼")]
            [Required(ErrorMessage="{0}為必填")]
            [StringLength(3,ErrorMessage="{0}字數不可超過{1}個字")]
            [RegularExpression(@"^[a-zA-Z'''-'\s]{3,3}$", ErrorMessage = "不正確的代碼")]
            public string City_no { get; set; }
            [DisplayName("索引值")]
            public int CityIndex { get; set; }
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}為必填")]
            public int CountryIndex { get; set; }
            [DisplayName("州/省別代碼")]
            [Required(ErrorMessage = "{0}為必填")]
            public int StateIndex { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string ShortName { get; set; }
            [DisplayName("中文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string Ename { get; set; }
            [DisplayName("電話區碼")]
            [StringLength(4, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_Area { get; set; }
            [DisplayName("機場類別")]
            [StringLength(10, ErrorMessage = "{0}字數不可超過{1}個字")]
            public string Airport_type { get; set; }
            [DisplayName("標準時間UTC")]
            public Nullable<double> UTC { get; set; }
            [DisplayName("夏令開始時間")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> Daylight_DateTime { get; set; }
            [DisplayName("顯示順序")]
            [Range(1,255,ErrorMessage="{0}只能在{1}到{2}之間")]
            [Required(ErrorMessage = "{0}為必填")]
            public Nullable<int> Order { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1,3,ErrorMessage="{0}只能在{1}到{2}之間")]
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