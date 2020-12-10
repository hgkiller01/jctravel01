using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public class Relevance
    {
        private TravelContainer db = new TravelContainer();
        public bool ValidateStatus(Country01 country,string company)
        {
            int count = 0;
            Country01 country02 = db.Country01.Find(country.CountryIndex);
            if (country02.State02 != null || country02.City03 != null || country02.AirlineOffice != null)
            {
                count = country02.State02.Where(x => x.Status == 1).Count() + country02.City03.Where(x => x.Status == 1).Count() + country02.AirlineOffice.Where(x => x.Status == 1).Count();
            }
            if (country.Status == 2 && country.CompanyNo == company && count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ValidateStatus(State02 state, string company)
        {
            int count =0;
            State02 state02 = db.State02.Find(state.StateIndex);
            if (state02.City03.Count() > 0)
            {
                count= state02.City03.Where(x => x.Status == 1).Count();
            }
            if (state.Status == 2 && state.CompanyNo == company && count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ValidateStatus(City03 city, string company)
        {
            int count = 0;
            City03 city02 = db.City03.Find(city.CityIndex);
            if (city.AirlineOffice != null)
            {
                count = city02.AirlineOffice.Count();
            }
            if (city.Status == 2 && city.CompanyNo == company && count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool ValidateStatus(Airline airline, string company)
        {
            int count = 0;
            Airline airline02 = db.Airline.Find(airline.AirlineIndex);
            if (airline.AirlineOffice != null)
            {
                count = airline02.AirlineOffice.Count();
            }
            if (airline.Status == 2 && airline.CompanyNo == company && airline02.AirlineOffice.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}