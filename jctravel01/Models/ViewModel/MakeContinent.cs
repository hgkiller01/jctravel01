using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class MakeContinent
    {
        public SelectList Countinet { get; set; }
        public List<string> ContinetList { get; set; }

        public MakeContinent()
        {
            this.ContinetList = new List<string>{
                "亞洲",
                "非洲",
                "北美洲",
                "南美洲",
                "南極洲",
                "歐洲",
                "大洋洲",
            };
            Countinet = new SelectList(ContinetList);  
        }
        public SelectList MakeNowContinent(string continent)
        {
            return new SelectList(ContinetList, selectedValue: continent);
        }
    }
}