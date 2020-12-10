using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class Sc_UploadImage
    {
        [FileExtensions(Extensions="jpg,png")]
        public HttpPostedFileBase imageUpload { get; set; }
        [Required()]
        [StringLength(70)]
        public string imgInfo { get; set; }
        public string cname { get; set; }
        public string scenery_no { get; set; }
        public string rest_no { get; set; }
        public string holtel_no { get; set; }
        public int scenery_index { get; set; }
        public int restIndex { get; set; }
        public int hotelIndex { get; set; }
       
    }
}