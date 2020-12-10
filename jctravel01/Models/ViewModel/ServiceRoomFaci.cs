using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class ServiceRoomFaci
    {
       private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("旅館代號")]
        public string Hotel_no { get; set; }
        public int HotelIndex { get; set; }
        [DisplayName("旅館名稱")]
        public string HotelName { get; set; }
        public List<HotelService> Service { get; set; }
        public List<RoomFacility> Facility { get; set; }
        public int[] Service_no { get; set; }
        public int[] Facility_no { get; set; }
        public MultiSelectList ServiceList { get; set; }
        public MultiSelectList FacilityList { get; set; }
        public ServiceRoomFaci()
        {
            Service = new List<HotelService>();
            Facility = new List<RoomFacility>();
        }
        public ServiceRoomFaci(string companyNo, int hotelIndex, int[] ServiceIndexes, int[] FacilityIndexes)
        {
            HotelIndex = hotelIndex;
            HotelName = db.Hotel.Find(hotelIndex).Cname;
            Hotel_no = db.Hotel.Find(hotelIndex).Holtel_no;
            CompanyNo = companyNo;
            ServiceList = new MultiSelectList(db.HotelSer_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Hotel_Ser_no", "Cname", ServiceIndexes);
            FacilityList = new MultiSelectList(db.RoomFaci_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "RoomFaci_no", "Cname", FacilityIndexes);
                
            
        }
    }
}