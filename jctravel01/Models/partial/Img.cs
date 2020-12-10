using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(ImgMD))]
    public partial class Img
    {
        public class ImgMD
        {
            public long ImgIndex { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("元件類別")]
            public int POI_Type { get; set; }
            [ScaffoldColumn(false)]
            public Nullable<int> Scenery_index { get; set; }
            [ScaffoldColumn(false)]
            public Nullable<int> RestIndex { get; set; }
            [ScaffoldColumn(false)]
            public Nullable<int> HotelIndex { get; set; }
            [DisplayName("元件圖片順序")]
            [Range(1,255)]
            [Required(ErrorMessage = "{0}必填")]
            public int OrderNo { get; set; }
            [DisplayName("觀看圖片")]
            public string ImgPath { get; set; }
            [DisplayName("圖片說明")]
            [StringLength(50)]
            [Required(ErrorMessage = "{0}必填")]
            public string ImgInfo { get; set; }
            [DisplayName("上傳者")]
            public int CreateBy { get; set; }
            [DisplayName("上傳時間")]
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