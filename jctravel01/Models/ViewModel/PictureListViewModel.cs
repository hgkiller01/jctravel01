using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public class PictureListViewModel
    {
        public Scenery  scenery { get; set; }
        public Hotel hotel { get; set; }
        public Restaurant restaurant { get; set; }
        public string Company { get; set; }
    }
}