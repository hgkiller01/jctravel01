using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class MemberLogin2
    {
        public int empIndex { get; set; }
        [DisplayName("現在位置")]
        [Required(ErrorMessage="必需要選擇目前位置")]
        public string AtWhere { get; set; }
        [DisplayName("其他位置")]
        [StringLength(10,ErrorMessage="不能超過10個字")]
        public string Location { get; set; }
        [DisplayName("身分證字號")]
        [Required()]
        [StringLength(20,ErrorMessage="不能超過20字")]
        public string empID { get; set; }
        [DisplayName("生日")]
        [Required()]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }
    }
}