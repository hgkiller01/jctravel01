using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(DepIndexMD))]
    public partial class DepIndex
    {
        public class DepIndexMD
        {
            public int Dep_Index { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("部門代號")]
            public string DepNo { get; set; }
            [DisplayName("部門名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20)]
            public string DepName { get; set; }
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
        public DepIndex Clone()
        {
            DepIndex dep = new DepIndex();
            dep.CompanyNo = this.CompanyNo;
            dep.CreateBy = this.CreateBy;
            dep.CreateBy_Time = this.CreateBy_Time;
            dep.UpdateBy = this.UpdateBy;
            dep.UpdateBy_Time = this.UpdateBy_Time;
            dep.Status = this.Status;
            return dep;
        }
    }
}