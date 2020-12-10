using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace jctravel01.Models.ViewModel
{
    public class UImage
    {
        public HttpPostedFileBase imageUpload { get; set; }

    }
}