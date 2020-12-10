using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class TakeUTC
    {
        public Dictionary<double,string> UTCTime { get; set; }
        public SelectList UTCSelect { get; set; }
        public TakeUTC()
        {
            UTCTime = new Dictionary<double, string>();
            for (double i = -12; i < 13; i+=0.5)
            {
                if (i > 0)
                {
                    UTCTime.Add(i, "+" + i);
                }
                else
                {
                    UTCTime.Add(i, i.ToString());
                }
            }
            UTCSelect = new SelectList(UTCTime, "key", "value",0);
        }
        public void SetNowSelect(double? selected)
        {
            UTCSelect = new SelectList(UTCTime, "key", "value",selected);
        }
    }
}