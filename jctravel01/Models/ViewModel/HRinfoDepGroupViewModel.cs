using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class HRinfoDepGroupViewModel
    {
        public int[] EmpIndex { get; set; }
        [DisplayName("部門")]
        public int? DepIndex { get; set; }
        [DisplayName("群組")]
        public int? JobGroup { get; set; }
        [DisplayName("權限")]
        public int[] PermiGroups { get; set; }
    }
}