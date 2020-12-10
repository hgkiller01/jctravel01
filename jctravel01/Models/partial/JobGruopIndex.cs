using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(JobGruopIndexMD))]
    public partial class JobGruopIndex
    {

        public class JobGruopIndexMD
        {
            public int JobGruop_Index { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("群組代號")]
            public string JobGruopNo { get; set; }
            [DisplayName("群組名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20)]
            public string JobGroupName { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
            [DisplayName("狀態")]
            public int Status { get; set; }
        }
    }
}