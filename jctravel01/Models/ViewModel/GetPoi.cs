using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jctravel01.Models.ViewModel
{
    public class GetPoi
    {
        public List<Poi_Type> poi { get; set; }
        public GetPoi()
        {
            poi = new List<Poi_Type>(){
               new Poi_Type(){
                   poiIndex = 1,
                   PoiName = "景點"
               },
               new Poi_Type(){
                   poiIndex =2,
                   PoiName ="餐廳"
               },
               new Poi_Type(){
                   poiIndex =3,
                   PoiName ="旅館"
               }
            };
        }
    }
}