using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(UpDivisionMD))]
    public partial class UpDivision
    {
        public class UpDivisionMD
        {
            [DisplayName("索引值")]
            public int PDivisionIndex { get; set; }
            [DisplayName("大線別代號")]
            [StringLength(1)]
            [Required(ErrorMessage = "{0}必填")]
            [RegularExpression("[0-9]*", ErrorMessage = "必需為數字")]
            public string PDivision_Code { get; set; }
            [DisplayName("名稱")]
            [StringLength(10)]
            [Required(ErrorMessage = "{0}必填")]
            public string Cname { get; set; }
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
        }
    }
}