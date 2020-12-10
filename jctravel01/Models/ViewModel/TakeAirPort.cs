using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class TakeAirPort
    {
        public Dictionary<string,string> AirPort { get; set; }
        public SelectList AirportList { get; set; }
        public TakeAirPort()
        {
            AirPort = new Dictionary<string, string>(){
            {"國際機場","國際機場"},
            {"國內機場","國內機場"},
            };
            AirportList = new SelectList(AirPort, "key", "value");
        }
        public void SetNowSelect(string NowPort)
        {
            AirportList = new SelectList(AirPort, "key", "value",NowPort);
        }
    }
}