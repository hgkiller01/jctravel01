using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class Star
    {
        public List<double> StarNum { get; set; }
        public SelectList  StarList { get; set; }
        public Star()
        {
            StarNum = new List<double>();
            for (double i = 1; i <= 5; i += 0.5)
            {
                StarNum.Add(i);
            }
            StarList = new SelectList(StarNum);
        }
        public void SetStar(double? star)
        {
            StarList = new SelectList(StarNum,star);
        }
    }
}