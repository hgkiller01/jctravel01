using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(SceneryMD))]
    public partial class Scenery
    {
        public class SceneryMD
        {
            public int Scenery_index { get; set; }
            [DisplayName("景點代碼")]
            public string Scenery_no { get; set; }
            [DisplayName("國家代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CountryIndex { get; set; }
            [DisplayName("城市代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public int CityIndex { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            public string ShortName { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string Ename { get; set; }
            [DisplayName("中文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("區域")]
            public Nullable<int> CityDistrictIndex { get; set; }
            [DisplayName("國碼")]
            [StringLength(6, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_CountryCode { get; set; }
            [DisplayName("區碼")]
            [StringLength(4, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string Tele_Area { get; set; }
            [DisplayName("電話")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Tele_number { get; set; }
            [DisplayName("傳真")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Fax { get; set; }
            [DisplayName("E-mail")]
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
            [DisplayName("地址")]
            [DataType(DataType.MultilineText)]
            public string Addr { get; set; }
            [DisplayName("景點介紹")]
            [DataType(DataType.MultilineText)]
            public string Introduction { get; set; }
            [DisplayName("景點費用")]
            [RegularExpression(@"^[A-Z]{3,3}[0-9]{0,}$", ErrorMessage = "費用必需是幣別+金額")]
            [StringLength(10)]
            public string Fee { get; set; }
            [DisplayName("位置描述")]
            [DataType(DataType.MultilineText)]
            public string Locate_des { get; set; }
            [DisplayName("設立西元年份")]
            [Range(1700,3000)]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public Nullable<int> Found_Date { get; set; }
            [DisplayName("建議停留時間")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public Nullable<int> Stay_Time { get; set; }
            [DisplayName("佔地面積")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public Nullable<int> Bulit_Area { get; set; }
            [DisplayName("建議遊玩方式")]
            [DataType(DataType.MultilineText)]
            public string Suggestion { get; set; }
            [DisplayName("開始入場時間")]
            [DataType(DataType.Time)]
            public Nullable<System.TimeSpan> Start_time { get; set; }
            [DisplayName("最晚入場時間")]
            [DataType(DataType.Time)]
            public Nullable<System.TimeSpan> LastCall { get; set; }
            [DataType(DataType.Time)]
            [DisplayName("結束時間")]
            public Nullable<System.TimeSpan> Close_time { get; set; }
            [DisplayName("班次表")]
            [DataType(DataType.MultilineText)]
            public string TimeSheet { get; set; }
            [DisplayName("公休日期")]
            [DataType(DataType.MultilineText)]
            public string Dayoff { get; set; }
            [DisplayName("推廌夜間賞玩")]
            public Nullable<bool> ForNight { get; set; }
            [DisplayName("安全等級")]
            [Range(1,3)]
            public Nullable<int> SafeLevel { get; set; }
            [DisplayName("主要景點")]
            public Nullable<bool> MainPoint { get; set; }
            [DisplayName("建檔狀態")]
            [Range(1,3)]
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
            [DefaultValue("00001")]
            public string CompanyNo { get; set; }
        }
    }
}