using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public static class GetSafeLevel
    {
        public static Dictionary<int,string> safeleveld { get; set; }
        public static SelectList safelevel()
        {
            safeleveld = new Dictionary<int, string>(){
            {1,"危險"},
            { 2,"有點危險"},
            {3,"安全"}
            };
            return new SelectList(safeleveld, "key", "value",3);
        }
        public static SelectList safelevel(int? key)
        {
            safelevel();
            return new SelectList(safeleveld, "key", "value",key);
        }
        public static string GetsafeLevel(int? safelevel)
        {
            string about = "";
            switch (safelevel)
            {
                case 1:
                    about= "危險";
                    break;
                case 2:
                    about= "有點危險";
                    break;
                case 3:
                    about= "安全";
                    break;
            }
            return about;
        }

    }
}