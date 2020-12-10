using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace jctravel01.Models.ViewModel
{
    public class DiningAmosViewModel
    {
        private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("餐廳代號")]
        public string rest_no { get; set; }
        public int restIndex { get; set; }
        [DisplayName("餐廳名稱")]
        public string ResName { get; set; }
        public List<ResDining> resDining { get; set; }
        public List<ResAmos> resAmos { get; set; }
        public int[] amosph_index { get; set; }
        public int[] diningSty_index { get; set; }
        public MultiSelectList amosList { get; set; }
        public MultiSelectList diningList { get; set; }

        public DiningAmosViewModel()
        {
            resDining = new List<ResDining>();
            resAmos = new List<ResAmos>();
        }
        public DiningAmosViewModel(string companyNo, int RestIndex, int[] Amosph_Indexes, int[] DiningSty_Indexes)
        {
            restIndex = RestIndex;
            ResName = db.Restaurant.Find(RestIndex).Cname;
            rest_no = db.Restaurant.Find(RestIndex).Rest_no;
            CompanyNo = companyNo;
            amosList = new MultiSelectList(db.AmosphIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 1), "Amosph_Index", "Cname", Amosph_Indexes);
            diningList = new MultiSelectList(db.DiningStyIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 1), "DiningSty_Index", "Cname", DiningSty_Indexes);
                          
        }
    }
}