using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public class AllGroup
    {
        public int EmpIndex { get; set; }
        public int PermiGpIndex { get; set; }
        public string PermiGpName { get; set; }
        public bool IsSelected { get; set; }
        public object Tags { get; set; }
    }
    public class PermiList
    {
        public int EmpIndex { get; set; }
        public IEnumerable<AllGroup> PermiGroupList { get; set; }
        public string[] GroupIndex { get; set; }
    }
}