using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class TypeThemeHViewModel
    {
        private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("旅館代號")]
        public string Hotel_no { get; set; }
        public int HotelIndex { get; set; }
        [DisplayName("旅館名稱")]
        public string HotelName { get; set; }
        public List<Hotel_Type> type1 { get; set; }
        public List<Hotel_Theme> theme1 { get; set; }
        public int[] Theme_index { get; set; }
        public int[] Type_index { get; set; }
        public MultiSelectList typeList { get; set; }
        public MultiSelectList themeList { get; set; }
        public TypeThemeHViewModel()
        {
            type1 = new List<Hotel_Type>();
            theme1 = new List<Hotel_Theme>();
        }
        public TypeThemeHViewModel(string companyNo, int hotelIndex, int[] TypeIndexes, int[] ThemeIndexes)
        {
            HotelIndex = hotelIndex;
            HotelName = db.Hotel.Find(hotelIndex).Cname;
            Hotel_no = db.Hotel.Find(hotelIndex).Holtel_no;
            CompanyNo = companyNo;
            typeList = new MultiSelectList(db.Type_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Type_Index1", "Cname", TypeIndexes);
            themeList = new MultiSelectList(db.Theme_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Theme_Index1", "Cname", ThemeIndexes);
                
            
        }
    }
}