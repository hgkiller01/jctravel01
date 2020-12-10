using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(PermiIndexMD))]
    public partial class PermiIndex
    {
        public class PermiIndexMD
        {
            public int Permilindex { get; set; }
            [DisplayName("權限代號")]
            public string PermiNo { get; set; }
            [DisplayName("主功能代號")]
            public string MainPerNo { get; set; }
            [DisplayName("主功能名稱")]
            public string MainPerName { get; set; }
            [DisplayName("次功能代號")]
            public string AltPerNo { get; set; }
            [DisplayName("次功能名稱")]
            public string AltPerName { get; set; }
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