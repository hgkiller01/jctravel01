using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(Scenery_ThemeMD))]
    public partial class Scenery_Theme
    {
        public class Scenery_ThemeMD
        {
            public int Scenery_ThemeIndex { get; set; }
            [DisplayName("景點代碼")]
            public int Scenery_index { get; set; }
            [DisplayName("主題代碼")]
            public int Theme_index { get; set; }
            public int CreateBy { get; set; }
            public System.DateTime CreateBy_Time { get; set; }
            public int UpdateBy { get; set; }
            public System.DateTime UpdateBy_Time { get; set; }
            public string CompanyNo { get; set; }
            public bool Main { get; set; }
        }
    }
}