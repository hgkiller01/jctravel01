using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(DivisionMD))]
    public partial class Division
    {
        public class DivisionMD
        {
            public int DivisionIndex { get; set; }
            [DisplayName("大線別代碼")]
            [Required(ErrorMessage = "{0}必填")]          
            public int PDivisionIndex { get; set; }
            [DisplayName("小線別代碼")]
            [Required(ErrorMessage = "{0}必填")]
            public string Division_code { get; set; }
            [DisplayName("線別名稱")]
            [StringLength(10)]
            [Required(ErrorMessage = "{0}必填")]
            [RegularExpression("^[\u4e00-\u9fffh]{0,}$", ErrorMessage = "只能輸入中文")]
            public string Cname { get; set; }
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
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
        }
    }
}