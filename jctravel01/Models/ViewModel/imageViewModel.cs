using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public class imageViewModel
    {
        public List<int?> Hotelindex { get; set; }
        public List<int?> Restindex { get; set; }
        public List<int?> Sceneryindex { get; set; }
        public List<Img> images { get; set; }
        public string CompnyNo { get; set; }
        public imageViewModel(string compnyNo)
        {
            CompnyNo = compnyNo;
            TravelContainer db = new TravelContainer();
            Hotelindex = db.Img.Select(x => x.HotelIndex).Where(x => x.HasValue).Distinct().ToList();
            Restindex = db.Img.Select(x => x.RestIndex).Where(x => x.HasValue).Distinct().ToList();
            Sceneryindex = db.Img.Select(x => x.Scenery_index).Where(x => x.HasValue).Distinct().ToList();
            images = new List<Img>();
            foreach (int i in Hotelindex)
            {
                var hotel = db.Img.Include(x => x.Hotel).Where(x => x.HotelIndex == i && x.CompanyNo == compnyNo).FirstOrDefault();
                if (hotel != null)
                {
                    images.Add(hotel);
                }               
            }
            foreach (int i in Restindex)
            {
                var rest = db.Img.Include(x => x.Restaurant).Where(x => x.RestIndex == i && x.CompanyNo == compnyNo).FirstOrDefault();
                if (rest != null)
                {
                    images.Add(rest);
                }   
            }
            foreach(int i in Sceneryindex)
            {
                var scenery = db.Img.Include(x => x.Scenery).Where(x => x.Scenery_index == i && x.CompanyNo == compnyNo).FirstOrDefault();
                if (scenery != null)
                {
                    images.Add(scenery);
                }
            }
        }
    }
}