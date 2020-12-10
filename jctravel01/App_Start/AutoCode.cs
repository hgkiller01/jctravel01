using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jctravel01.Models;

namespace jctravel01
{
    public class AutoCode
    {
        public string GetAutoCode(string officecode,string countrycode,string citycode) 
        {
            string all = officecode + countrycode + citycode;
            
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();
            using (TravelContainer db = new TravelContainer())
            {
                var AirCode = db.AirlineOffice.Where(x => x.CompanyNo == compyNo && x.ALoffice_Code.StartsWith(all));
                int count = AirCode.Count();
                if (count > 0)
                {
                    string code = AirCode.Select(x => new { x.ALoffice_Code }).OrderByDescending(x => x.ALoffice_Code).FirstOrDefault().ALoffice_Code;
                    code = code.Substring(code.Length - 2, 2);
                   return all + (int.Parse(code)+1).ToString("00");
                }
                else
                {
                    return all + string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeP(string countrycode, string citycode)
        {
            string all = countrycode + citycode + "P";
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();
            using (TravelContainer db = new TravelContainer())
            {
                var scenery = db.Scenery.Where(x => x.CompanyNo == compyNo && x.Scenery_no.StartsWith(all));
                int count = scenery.Count();
                if (count > 0)
                {
                    string code = scenery.Select(x => new { x.Scenery_no }).OrderByDescending(x => x.Scenery_no).FirstOrDefault().Scenery_no;
                    code = code.Substring(code.Length - 5, 5);
                    return all + (int.Parse(code) + 1).ToString("00000");
                }
                else
                {
                    return all + string.Format("{0:00000}", 1);
                }
            }
        }
        public string GetAutoCodeH(string countrycode, string citycode)
        {
            string all = countrycode + citycode + "H";
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();
            using (TravelContainer db = new TravelContainer())
            {
                var scenery = db.Hotel.Where(x => x.CompanyNo == compyNo && x.Holtel_no.StartsWith(all));
                int count = scenery.Count();
                if (count > 0)
                {
                    string code = scenery.Select(x => new { x.Holtel_no }).OrderByDescending(x => x.Holtel_no).FirstOrDefault().Holtel_no;
                    code = code.Substring(code.Length - 5, 5);
                    return all + (int.Parse(code) + 1).ToString("00000");
                }
                else
                {
                    return all + string.Format("{0:00000}", 1);
                }
            }
        }
        public string GetAutoCodeR(string countrycode, string citycode)
        {
            string all = countrycode + citycode + "R";
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();
            using (TravelContainer db = new TravelContainer())
            {
                var scenery = db.Restaurant.Where(x => x.CompanyNo == compyNo && x.Rest_no.StartsWith(all));
                int count = scenery.Count();
                if (count > 0)
                {
                    string code = scenery.Select(x => new { x.Rest_no }).OrderByDescending(x => x.Rest_no).FirstOrDefault().Rest_no;
                    code = code.Substring(code.Length - 5, 5);
                    return all + (int.Parse(code) + 1).ToString("00000");
                }
                else
                {
                    return all + string.Format("{0:00000}", 1);
                }
            }
        }
        public string GetAutoCodeT(string countrycode)
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();
            using (TravelContainer db = new TravelContainer())
            {
                var tourBure = db.TourBure.Where(x => x.CompanyNo == compyNo && x.TourBure_no.StartsWith(countrycode));
                int count = tourBure.Count();
                if (count > 0)
                {
                    string code = tourBure.Select(x => new { x.TourBure_no }).OrderByDescending(x => x.TourBure_no).FirstOrDefault().TourBure_no;
                    code = code.Substring(code.Length - 5, 5);
                    return countrycode + (int.Parse(code) + 1).ToString("00000");
                }
                else
                {
                    return countrycode + string.Format("{0:00000}", 1);
                }
            }
        }
        public string GetAutoCodeArea(string citycode)
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var citydistrict = db.CityDistrict.Where(x => x.CompanyNo == compyNo && x.CityDistrictCode.StartsWith(citycode));
                int count = citydistrict.Count();
                if (count > 0)
                {
                    string code = citydistrict.Select(x => new { x.CityDistrictCode }).OrderByDescending(x => x.CityDistrictCode).FirstOrDefault().CityDistrictCode;
                    code = code.Substring(code.Length - 3, 3);
                    return citycode + (int.Parse(code) + 1).ToString("000");
                }
                else
                {
                    return citycode + string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeTheme()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var theme= db.Theme_index.OrderByDescending(x => x.Theme_Code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (theme !=null)
                {
                    return (int.Parse(theme.Theme_Code) + 1).ToString("000");
                }
                else
                {
                    return string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeType()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowType = db.Type_index.OrderByDescending(x => x.Type_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowType != null)
                {
                    return (int.Parse(NowType.Type_code) + 1).ToString("000");
                }
                else
                {
                    return string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeDining()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowType = db.DiningStyIndex.OrderByDescending(x => x.DiningSty_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowType != null)
                {
                    return (int.Parse(NowType.DiningSty_code) + 1).ToString("000");
                }
                else
                {
                    return string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeAmos()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowAmos = db.AmosphIndex.OrderByDescending(x => x.Amosph_no).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowAmos != null)
                {
                    return (int.Parse(NowAmos.Amosph_no) + 1).ToString("000");
                }
                else
                {
                    return string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeHotelFaci()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowHotelFaci = db.HotelFaci_index.OrderByDescending(x => x.HotelFaci_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowHotelFaci != null)
                {
                    return (int.Parse(NowHotelFaci.HotelFaci_code) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeNearFaci()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowNearFaci = db.NearbyFcai_index.OrderByDescending(x => x.NearbyFaci_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowNearFaci != null)
                {
                    return (int.Parse(NowNearFaci.NearbyFaci_code) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeHotelSer()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowHotelSer = db.HotelSer_index.OrderByDescending(x => x.Hotel_Ser_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowHotelSer != null)
                {
                    return (int.Parse(NowHotelSer.Hotel_Ser_code) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeRoomFaci()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowRoomFaci = db.RoomFaci_index.OrderByDescending(x => x.RoomFaci_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowRoomFaci != null)
                {
                    return (int.Parse(NowRoomFaci.RoomFaci_code) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeRoomType()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowRoomType = db.RoomType_index.OrderByDescending(x => x.RoomType_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowRoomType != null)
                {
                    return (int.Parse(NowRoomType.RoomType_code) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeOutLook()
        {
            string compyNo = HttpContext.Current.Session["ComnpanyNo"].ToString();

            using (TravelContainer db = new TravelContainer())
            {
                var NowOutLook = db.OutLook_index.OrderByDescending(x => x.OutLook_code).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowOutLook != null)
                {
                    return (int.Parse(NowOutLook.OutLook_code) + 1).ToString("000");
                }
                else
                {
                    return string.Format("{0:000}", 1);
                }
            }
        }
        public string GetAutoCodeCompy()
        {

            using (TravelContainer db = new TravelContainer())
            {
                var compny = db.CoIndex.OrderByDescending(x => x.CompanyNo).FirstOrDefault();
                if (compny != null)
                {
                    return (int.Parse(compny.CompanyNo) + 1).ToString("00000");
                }
                else
                {
                    return string.Format("{0:00000}", 1);
                }
            }
        }
        public string GetAutoCodeDep(string CompyNo)
        {
            string compyNo = CompyNo;

            using (TravelContainer db = new TravelContainer())
            {
                var NowDep = db.DepIndex.OrderByDescending(x => x.DepNo).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowDep != null)
                {
                    return (int.Parse(NowDep.DepNo) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeJobGroup(string CompyNo)
        {
            string compyNo = CompyNo;

            using (TravelContainer db = new TravelContainer())
            {
                var NowJob = db.JobGruopIndex.OrderByDescending(x => x.JobGruopNo).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowJob != null)
                {
                    return (int.Parse(NowJob.JobGruopNo) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }

        public string GetAutoCodeJobLevel(string CompyNo)
        {
            string compyNo = CompyNo;

            using (TravelContainer db = new TravelContainer())
            {
                var NowLevel = db.JobLevelIndex.OrderByDescending(x => x.JobLevelCode).Where(x => x.CompanyNo == compyNo).FirstOrDefault();
                if (NowLevel != null)
                {
                    return (int.Parse(NowLevel.JobLevelCode) + 1).ToString("00");
                }
                else
                {
                    return string.Format("{0:00}", 1);
                }
            }
        }
        public string GetAutoCodeHR(string CompyNo)
        {
            List<string> EngAlp = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
            using (TravelContainer db = new TravelContainer())
            {
                var hRinfo = db.HRInfo.OrderByDescending(x => x.EmpNo).Where(x => x.CompanyNo == CompyNo).FirstOrDefault();
                if (hRinfo != null)
                {
                    string NowNo = hRinfo.EmpNo.Substring(1, 4);
                    string alp = hRinfo.EmpNo.Substring(0, 1);
                    if (Convert.ToInt32(NowNo) == 9999)
                    {
                      int nowIndex =  EngAlp.FindIndex(x => x == alp);
                      return EngAlp.ElementAt(nowIndex) + "0001";
                    }
                    else
                    {
                        return alp + ((Convert.ToInt32(NowNo) + 1).ToString("0000"));
                    }
                }
                else
                {
                    return "A" + string.Format("{0:0000}", 1);
                }
            }
            
        }

    }
}