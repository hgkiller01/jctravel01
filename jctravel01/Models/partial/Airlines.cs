using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(AirlineMD))]
    public partial class Airline
    {
        public class AirlineMD
        {
            [Required(ErrorMessage = "{0}必填")]
            [DisplayName("航空公司代碼")]
            [StringLength(2)]
            [RegularExpression(@"^[a-zA-Z]{2,2}$", ErrorMessage = "不正確的代碼")]
            public string Airline_Code { get; set; }
            [DisplayName("索引值")]
            public int AirlineIndex { get; set; }
            [DisplayName("中文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
            [DisplayName("顯示名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string ShortName { get; set; }
            [DisplayName("英文名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(30, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[\sa-zA-Z]*", ErrorMessage = "只能輸入大小寫A-Z")]
            public string Ename { get; set; }
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
            [DisplayName("網站")]
            [StringLength(100)]
            [DataType(DataType.Url)]
            [RegularExpression(@"^(ht|f)tp(s?)\:\/\/(([a-zA-Z0-9\-\._]+(\.[a-zA-Z0-9\-\._]+)+)|localhost)(\/?)([a-zA-Z0-9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?([\d\w\.\/\%\+\-\=\&amp;\?\:\\\&quot;\'\,\|\~\;]*)$", ErrorMessage = "無效的URL")]
            public string URL { get; set; }
            [DisplayName("傳真")]
            [StringLength(15, ErrorMessage = "{0}字數不可超過{1}個字")]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string Fax { get; set; }
            [DisplayName("E-mail")]
            [StringLength(30)]
            [DataType(DataType.EmailAddress)]
            [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4})$",ErrorMessage="無效的E-Mail")]
            public string Email { get; set; }
            [DisplayName("建檔狀態")]
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