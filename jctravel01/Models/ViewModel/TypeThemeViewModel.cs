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
    public class TypeThemeViewModel
    {
        private TravelContainer db = new TravelContainer();
        public string CompanyNo { get; set; }
        [DisplayName("景點代號")]
        public string SceneryNo { get; set; }
        public int scenery_index { get; set; }
        [DisplayName("景點名稱")]
        public string sceneryName { get; set; }
        public List<Scenery_Type> type1 { get; set; }
        public List<Scenery_Theme> theme1 { get; set; }
        public int[] Theme_index { get; set; }
        public int[] Type_index { get; set; }
        public MultiSelectList typeList { get; set; }
        public MultiSelectList themeList { get; set; }
        public TypeThemeViewModel()
        {
            type1 = new List<Scenery_Type>();
            theme1 = new List<Scenery_Theme>();
        }
        public TypeThemeViewModel(string companyNo, int Scenery_index,int[] TypeIndexes,int[] ThemeIndexes)
        {
            scenery_index = Scenery_index;
            sceneryName = db.Scenery.Find(scenery_index).Cname;
            SceneryNo = db.Scenery.Find(scenery_index).Scenery_no;
            CompanyNo = companyNo;
            typeList = new MultiSelectList(db.Type_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Type_Index1", "Cname", TypeIndexes);
            themeList = new MultiSelectList(db.Theme_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 1).ToList(), "Theme_Index1", "Cname", ThemeIndexes);
                
            
        }
    }
}