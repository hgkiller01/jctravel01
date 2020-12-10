using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class PermiGruopViewModel
    {
        [DisplayName("群組代號")]
        [StringLength(20)]
        [Required()]
        public string PermiGpNo { get; set; }
        [DisplayName("群組名稱")]
        [StringLength(6)]
        [Required()]
        public string PermiGpName { get; set; }
        [DisplayName("說明")]
        [StringLength(20)]
        [Required()]
        public string Descri { get; set; }
    }
}