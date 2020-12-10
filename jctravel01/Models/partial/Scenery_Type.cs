using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace jctravel01.Models.partial
{
    [MetadataType(typeof(Scenery_TypeMD))]
    public partial class Scenery_Type
    {
        public class Scenery_TypeMD
        {
            public int Scenery_TypeIndex { get; set; }
            [DisplayName("景點代碼")]
            public int Scenery_index { get; set; }
            [DisplayName("型態代碼")]
            public int Type_index { get; set; }
            public int CreateBy { get; set; }
            public System.DateTime CreateBy_Time { get; set; }
            public int UpdateBy { get; set; }
            public System.DateTime UpdateBy_Time { get; set; }
            public string CompanyNo { get; set; }
            public bool Main { get; set; }
        }
    }
}