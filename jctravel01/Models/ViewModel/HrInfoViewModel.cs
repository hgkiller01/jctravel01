using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class HrInfoViewModel
    {
        public int EmpIndex { get; set; }
        [DisplayName("員工編號")]
        public string EmpNo { get; set; }
        [ScaffoldColumn(false)]
        public string EmpEnName { get; set; }
        [DisplayName("中文姓名")]
        [Required()]
        [StringLength(20)]
        public string EmpName { get; set; }
        [DisplayName("主管")]
        public string JobManager { get; set; }
        [DisplayName("職稱")]
        [Required()]
        public string JobTitle { get; set; }
        [DisplayName("在職狀態")]
        [Required()]
        public int OnJobStatus { get; set; }
        [DisplayName("公司電話")]
        [Required()]
        [StringLength(12)]
        [RegularExpression(@"[0-9\-]*")]
        public string ComPhone { get; set; }
        [DisplayName("分機")]
        [StringLength(10)]
        public string ComExt { get; set; }
    }
}