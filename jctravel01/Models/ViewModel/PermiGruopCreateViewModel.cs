using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class PermiGruopCreateViewModel
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
        public List<Permi> POIPermi { get; set; }
        public List<Permi> TourPermi { get; set; }
        public List<Permi> SalesPermi { get; set; }
        public List<Permi> EmpPermi { get; set; }
        public List<Permi> PermiPermi { get; set; }
        public List<Permi> FrontEndPermi { get; set; }
        public List<Permi> BasicPermi { get; set; }
        public List<Permi> AccountPermi { get; set; }
        public PermiGruopCreateViewModel() 
        {
            POIPermi = new List<Permi>();
            TourPermi = new List<Permi>();
            SalesPermi = new List<Permi>();
            EmpPermi = new List<Permi>();
            PermiPermi = new List<Permi>();
            BasicPermi = new List<Permi>();
            FrontEndPermi = new List<Permi>();
            AccountPermi = new List<Permi>();
        }
    }
}