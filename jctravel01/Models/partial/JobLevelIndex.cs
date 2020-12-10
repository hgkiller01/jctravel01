using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace jctravel01.Models
{
    [MetadataType(typeof(JobLevelIndexMD))]
    public partial class JobLevelIndex
    {
        public class JobLevelIndexMD
        {
            public int JobLevel_Index { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("職等代號")]
            public string JobLevelCode { get; set; }
            [DisplayName("職等名稱")]
            [Required(ErrorMessage = "{0}為必填")]
            [StringLength(20)]
            public string JobLevel { get; set; }
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
        public JobLevelIndex Clone()
        {
            JobLevelIndex JobL = new JobLevelIndex();
            JobL.CompanyNo = this.CompanyNo;
            JobL.CreateBy = this.CreateBy;
            JobL.CreateBy_Time = this.CreateBy_Time;
            JobL.UpdateBy = this.UpdateBy;
            JobL.UpdateBy_Time = this.UpdateBy_Time;
            JobL.Status = this.Status;
            return JobL;
        }
    }
}