using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(Authorze_indexMD))]
    public partial class Authorze_index
    {
        public class Authorze_indexMD
        {
            public int AutIndex { get; set; }
            [DisplayName("功能")]
            public int PermiIndex { get; set; }
            public int CreateBy { get; set; }
            public System.DateTime CreateBy_Time { get; set; }
            public int UpdateBy { get; set; }
            public System.DateTime UpdateBy_Time { get; set; }
        }
    }
}