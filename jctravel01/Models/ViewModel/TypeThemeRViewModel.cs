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
    public class TypeThemeRViewModel
    {
        private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("餐廳代號")]
        public string rest_no { get; set; }
        public int restIndex { get; set; }
        [DisplayName("餐廳名稱")]
        public string ResName { get; set; }
        public List<Res_Type> type1 { get; set; }
        public List<Res_Theme> theme1 { get; set; }
        public int[] Theme_index { get; set; }
        public int[] Type_index { get; set; }
        public MultiSelectList typeList { get; set; }
        public MultiSelectList themeList { get; set; }
        public TypeThemeRViewModel()
        {
            type1 = new List<Res_Type>();
            theme1 = new List<Res_Theme>();
        }
        public TypeThemeRViewModel(string companyNo, int RestIndex, int[] TypeIndexes, int[] ThemeIndexes)
        {
            restIndex = RestIndex;
            ResName = db.Restaurant.Find(RestIndex).Cname;
            rest_no = db.Restaurant.Find(RestIndex).Rest_no;
            CompanyNo = companyNo;
            typeList = new MultiSelectList(db.Type_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Type_Index1", "Cname", TypeIndexes);
            themeList = new MultiSelectList(db.Theme_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Theme_Index1", "Cname", ThemeIndexes);
                
            
        }
    }
}