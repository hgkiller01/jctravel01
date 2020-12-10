using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(PermiGruopMD))]
    public partial class PermiGruop
    {
        public class PermiGruopMD
        {
            public long PermiGpIndex { get; set; }
            [DisplayName("群組代號")]
            [StringLength(20)]
            [Required(ErrorMessage = "{0}必填")]
            public string PermiGpNo { get; set; }
            [DisplayName("群組名稱")]
            [StringLength(6)]
            [Required(ErrorMessage = "{0}必填")]
            public string PermiGpName { get; set; }
            [DisplayName("說明")]
            [StringLength(20)]
            [Required(ErrorMessage = "{0}必填")]
            public string Descri { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
            public string CompanyNo { get; set; }
        }
    }
}