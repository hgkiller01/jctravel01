using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class Plug
    {
        public List<string> plug { get; set; }
        public SelectList plugList { get; set; }
        public Plug()
        {
            plug = new List<string>(){
                "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O"
            };
            plugList = new SelectList(plug);
        }
        public Plug(string pg)
        {
            plug = new List<string>(){
                "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O"
            };
            plugList = new SelectList(plug,pg);
        }
    }
}