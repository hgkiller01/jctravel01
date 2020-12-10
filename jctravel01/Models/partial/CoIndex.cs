using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace jctravel01.Models
{
    [MetadataType(typeof(CoIndexMD))]
    public partial class CoIndex
    {
        public class CoIndexMD
        {
            [DisplayName("公司代號")]
            [ScaffoldColumn(true)]
            public string CompanyNo { get; set; }
            [DisplayName("公司名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(50)]
            public string ComName { get; set; }
            [DisplayName("統一編號")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(10)]
            [RegularExpression(@"[0-9]*", ErrorMessage = "必需為數字")]
            public string TaxID { get; set; }
            [DisplayName("連絡電話")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20)]
            [RegularExpression(@"[0-9\-]*", ErrorMessage = "必需為數字")]
            public string ContactPhone { get; set; }
            [DisplayName("連絡人")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20)]
            public string ContactName { get; set; }
            [DisplayName("地址")]
            [Required(ErrorMessage = "{0}為必填")]
            [DataType(DataType.MultilineText)]
            public string Co_addres { get; set; }
            [DisplayName("公司IP")]
            [CustomValidation(typeof(IPValidator),"CheckIP")]
            public string CoIP { get; set; }
            [DisplayName("建立者")]
            [ScaffoldColumn(false)]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            [ScaffoldColumn(false)]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            [ScaffoldColumn(false)]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            [ScaffoldColumn(false)]
            public System.DateTime UpdateBy_Time { get; set; }
            [DisplayName("使用狀態")]
            public bool Status { get; set; }
        }
    }
    public class IPValidator
    {
        public static ValidationResult CheckIP(string CoIP, ValidationContext Context)
        {
            IPAddress ip;
            if (string.IsNullOrEmpty(CoIP))
            {
                return ValidationResult.Success;
            }
            if (IPAddress.TryParse(CoIP, out ip))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("請輸入合法IP");
            }
        }
    }
}