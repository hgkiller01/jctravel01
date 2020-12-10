using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;
using jctravel01.Models.ViewModel;

namespace jctravel01
{
    public class CreateList 
    {
        public SelectList CreatedList { get; set; }
        public Dictionary<int,string> MyList { get; set; }
        public CreateList()
        {
            MyList = new Dictionary<int, string>();
        }
        public void addItem(int selectValue, string selectText)
        {
            MyList.Add(selectValue, selectText);

        }
        public void setList(int? id)
        {
            if (id == null)
            {
                CreatedList = new SelectList(MyList, "key", "value");
            }
            else
            {
                CreatedList = new SelectList(MyList, "key", "value",id);
            }
        }


    }
}