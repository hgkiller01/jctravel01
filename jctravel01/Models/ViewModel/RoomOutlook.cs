using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class RoomOutlook
    {
         private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("旅館代號")]
        public string Hotel_no { get; set; }
        public int HotelIndex { get; set; }
        [DisplayName("旅館名稱")]
        public string HotelName { get; set; }
        public List<RoomType> roomType { get; set; }
        public List<HotelOutLook> hotelOutLook { get; set; }
        public int[] RoomType_no { get; set; }
        public int[] OutLook_no { get; set; }
        public MultiSelectList RoomTypeList { get; set; }
        public MultiSelectList OutLookList { get; set; }
        public RoomOutlook()
        {
            roomType = new List<RoomType>();
            hotelOutLook = new List<HotelOutLook>();
        }
        public RoomOutlook(string companyNo, int hotelIndex, int[] RoomIndexes, int[] OutLookIndexes)
        {
            HotelIndex = hotelIndex;
            HotelName = db.Hotel.Find(hotelIndex).Cname;
            Hotel_no = db.Hotel.Find(hotelIndex).Holtel_no;
            CompanyNo = companyNo;
            RoomTypeList = new MultiSelectList(db.RoomType_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "RoomType_index1", "Cname", RoomIndexes);
            OutLookList = new MultiSelectList(db.OutLook_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "OutLook_index1", "Cname", OutLookIndexes);
                
            
        }
    }
}