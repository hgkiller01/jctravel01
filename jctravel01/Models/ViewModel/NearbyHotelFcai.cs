using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class NearbyHotelFcai
    {
        private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("旅館代號")]
        public string Hotel_no { get; set; }
        public int HotelIndex { get; set; }
        [DisplayName("旅館名稱")]
        public string HotelName { get; set; }
        public List<NearbyFacility> Nearby { get; set; }
        public List<HotelFacility> HotelFaci { get; set; }
        public int[] NearbyFaci_no { get; set; }
        public int[] HotelFaci_no { get; set; }
        public MultiSelectList NearbyList { get; set; }
        public MultiSelectList HotelFaciList { get; set; }
        public NearbyHotelFcai()
        {
            Nearby = new List<NearbyFacility>();
            HotelFaci = new List<HotelFacility>();
        }
        public NearbyHotelFcai(string companyNo, int hotelIndex, int[] NearbyFaciIndexes, int[] HotelFaciIndexes)
        {
            HotelIndex = hotelIndex;
            HotelName = db.Hotel.Find(hotelIndex).Cname;
            Hotel_no = db.Hotel.Find(hotelIndex).Holtel_no;
            CompanyNo = companyNo;
            NearbyList = new MultiSelectList(db.NearbyFcai_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "NearbyFaci_no", "Cname", NearbyFaciIndexes);
            HotelFaciList = new MultiSelectList(db.HotelFaci_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "HotelFaci_no", "Cname", HotelFaciIndexes);
                
            
        }
    }
}