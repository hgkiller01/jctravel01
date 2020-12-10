using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(RestaurantMD))]
    public partial class Restaurant
    {
        public class RestaurantMD
        {
            [DisplayName("餐廳代碼")]
            public string Rest_no { get; set; }
            [ScaffoldColumn(false)]
            public int RestIndex { get; set; }
            [ScaffoldColumn(true)]
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CountryIndex { get; set; }
            [ScaffoldColumn(true)]
            [DisplayName("城市代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CityIndex { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            public string ShortName { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(50, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string Ename { get; set; }
            [DisplayName("中文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"^[\s\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("區域")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public Nullable<int> CityDistrictIndex { get; set; }
            [DisplayName("國碼")]
            [StringLength(6, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_CountryCode { get; set; }
            [DisplayName("區碼")]
            [StringLength(4, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_Area { get; set; }
            [DisplayName("電話1")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Tele1 { get; set; }
            [DisplayName("電話2")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Tele2 { get; set; }
            [DisplayName("連絡人1")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"^[a-zA-Z\u4e00-\u9fffh\s]*", ErrorMessage = "不正確的姓名")]
            public string Contact1 { get; set; }
            [DisplayName("連絡人1電話")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Contact1_tele { get; set; }
            [DisplayName("連絡人2")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"^[a-zA-Z\u4e00-\u9fffh\s]*", ErrorMessage = "不正確的姓名")]
            public string Contact2 { get; set; }
            [DisplayName("連絡人2電話")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Contact2_tele { get; set; }
            [DisplayName("傳真")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Fax { get; set; }
            [DisplayName("E-Mail")]
            [StringLength(30)]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$", ErrorMessage = "無效的E-Mail")]
            public string Email { get; set; }
            [DisplayName("網站")]
            [StringLength(100)]
            [DataType(DataType.Url)]
            [RegularExpression(@"^(ht|f)tp(s?)\:\/\/(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$", ErrorMessage = "無效的URL")]
            public string URL { get; set; }
            [DisplayName("部落格")]
            [StringLength(100)]
            [DataType(DataType.Url)]
            [RegularExpression(@"^(ht|f)tp(s?)\:\/\/(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$", ErrorMessage = "無效的URL")]
            public string Blog { get; set; }
            [DisplayName("經度")]
            public Nullable<double> Longitude { get; set; }
            [DisplayName("緯度")]
            public Nullable<double> Latitude { get; set; }
            [DisplayName("菜系")]
            [StringLength(30, ErrorMessage = "{0}不能超過{1}個字")]
            public string Cuisine { get; set; }
            [DisplayName("開始營業時間")]
            [DataType(DataType.Time)]
            public Nullable<System.TimeSpan> Open_Time { get; set; }
            [DisplayName("結束營業時間")]
            [DataType(DataType.Time)]
            public Nullable<System.TimeSpan> Close_Time { get; set; }
            [DisplayName("推廌夜間遊玩")]
            public Nullable<bool> ForNight { get; set; }
            [DisplayName("限制用餐時間")]
            public Nullable<int> TimeLimit { get; set; }
            [DisplayName("地址")]
            [DataType(DataType.MultilineText)]
            public string Addr { get; set; }
            [DisplayName("介紹")]
            [DataType(DataType.MultilineText)]
            public string Introduction { get; set; }
            [DisplayName("位置描述")]
            [DataType(DataType.MultilineText)]
            public string Locate_des { get; set; }
            [DisplayName("風味餐")]
            [DataType(DataType.MultilineText)]
            public string Cuisine_Set { get; set; }
            [DisplayName("參考菜單")]
            [DataType(DataType.MultilineText)]
            public string Menu { get; set; }
            [DisplayName("公休日期")]
            [DataType(DataType.MultilineText)]
            public string Dayoff { get; set; }
            [DisplayName("建檔狀態")]
            [Required(ErrorMessage = "{0}必填")]
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