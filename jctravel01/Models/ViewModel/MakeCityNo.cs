using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public static class MakeCityNo
    {
        public static SelectList GetCityList()
        {
            TravelContainer db = new TravelContainer();
            
                return new SelectList(db.Country01.Where(x => x.Status == 1 || x.CompanyNo == "00001"), "Country_no", "Cname");
            
        }
        public static SelectList GetCityList(string country_no)
        {
            TravelContainer db = new TravelContainer();

            return new SelectList(db.Country01.Where(x => x.Status == 1 || x.CompanyNo == "00001"), "Country_no", "Cname", country_no);
           
        }
    }
}