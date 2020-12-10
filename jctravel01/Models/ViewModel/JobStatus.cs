using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jctravel01.Models.ViewModel
{
    public class JobStatus
    {
        public Dictionary<int, string> status { get; set; }
        public JobStatus() 
        {
            status = new Dictionary<int, string>();
            status.Add(1, "在職");
            status.Add(2, "留職停薪");
            status.Add(3, "育嬰假");
            status.Add(4, "離職");       
        }
    }
}