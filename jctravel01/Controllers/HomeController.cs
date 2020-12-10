using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;
using System.Web.Security;
using jctravel01.Models.ViewModel;

namespace jctravel01.Controllers
{
    public class HomeController : Controller
    {
        TravelContainer db = new TravelContainer();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
            FormsIdentity id = (FormsIdentity)User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            ViewBag.ticket = ticket.UserData;
            }

            return View();
        }
        public ActionResult Father()
        {
            return View();
        }
        public ActionResult Children()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
 
            return View();
        }
        public ActionResult CantPermit()
        {
            TempData["PermitError"] = "權限不足";
            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {

            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
            Session.Clear();
            TempData["LogOut"] = "您已經登出";
            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetCount()
        {
             LayoutViewModel LVM = new LayoutViewModel();
            if (Session["ComnpanyNo"] != null)
            {
                string CompanyNo = Session["ComnpanyNo"].ToString();
                LVM.CountryCount = db.Country01.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.StateCount = db.State02.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.CityCount = db.City03.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.AirLineInfoCount = db.Airline.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.AirLineOfficeCount = db.AirlineOffice.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.ViewPointCount = db.Scenery.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelCount = db.Hotel.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.RestCount = db.Restaurant.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.TourbCount = db.TourBure.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.AirPortCount = db.AirportInfo.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.CityAreaCount = db.CityDistrict.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.TypeCount = db.Type_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.ThemeCout = db.Theme_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.UpDivisCount = db.UpDivision.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.DivisCount = db.Division.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelFaciCount = db.HotelFaci_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelNearbyCount = db.NearbyFcai_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelSerCount = db.HotelSer_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelRoomFaciCount = db.RoomFaci_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelRoomCount = db.RoomType_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.HotelOutLookCount = db.OutLook_index.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.RestDinngCount = db.DiningStyIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                LVM.ResAmosCount = db.AmosphIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 2).Count();
                return Json(LVM, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return new HttpNotFoundResult();
            }
            
        }
    }
}