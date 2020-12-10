using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;
using jctravel01.Models.ViewModel;
using PagedList;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeCompny]
    public class AirlineOfficesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5; //每個分頁的資料數量
        // GET: AirlineOffices
        public ActionResult Index(int? Status,int? CountryIndex,string Cname,string Airline_Code ,string ALoffice_Code,int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var airlineOffice = db.AirlineOffice.OrderBy(x => x.AirlineOfficeIndex).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo==Company).AsNoTracking();
            if (!string.IsNullOrEmpty(Airline_Code))
            {
                ViewBag.Airline_Code = Airline_Code;
                airlineOffice = airlineOffice.Where(x => x.Airline.Airline_Code.Contains(Airline_Code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                airlineOffice = airlineOffice.Where(x => x.Airline.Cname.Contains(Cname) || x.Airline.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(ALoffice_Code))
            {
                ViewBag.ALoffice_Code = ALoffice_Code;
                airlineOffice = airlineOffice.Where(x => x.ALoffice_Code.Contains(ALoffice_Code));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                airlineOffice = airlineOffice.Where(x => x.Status == Status);
            }
            if (CountryIndex != null)
            {
                ViewBag.CountryIndex = CountryIndex;
                airlineOffice = airlineOffice.Where(x => x.CountryIndex == CountryIndex);
            }
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndexNum = new SelectList(country, "CountryIndex", "Cname");
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            ViewData["DataCount"] = airlineOffice.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = airlineOffice.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: AirlineOffices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlineOffice airlineOffice = db.AirlineOffice.Find(id);
            if (airlineOffice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(airlineOffice.Status);
            return View(airlineOffice);
        }

        // GET: AirlineOffices/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var firstCountry = country.First();
            var airline = db.Airline.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.AirlineIndex, Cname = x.Airline_Code + " " + x.Cname, x.AirlineOfficeIndex });
            ViewBag.AirlineIndex = new SelectList(airline, "AirlineIndex", "Cname");
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", firstCountry);
            var city = db.City03.Where(x => x.Country01.CountryIndex == firstCountry.CountryIndex && x.Status == 1).Select(x => new {x.CityIndex,Cname = x.City_no+" "+x.Cname});
            ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname");
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            return View();
        }

        // POST: AirlineOffices/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ALoffice_Code,AirlineIndex,AirlineOfficeIndex,CountryIndex,CityIndex,Tele_Order,Office_Number,Office_Fax,Office_Addr,Office_Mailbox,Status")] AirlineOffice airlineOffice)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode ac = new AutoCode();
            string airlinCode = db.Airline.Find(airlineOffice.AirlineIndex).Airline_Code;
            string countryCode = db.Country01.Find(airlineOffice.CountryIndex).Country_no;
            string cityCode = db.City03.Find(airlineOffice.CityIndex).City_no;
            airlineOffice.ALoffice_Code = ac.GetAutoCode(airlinCode,countryCode, cityCode);
            airlineOffice.CreateBy_Time = DateTime.Now;
            airlineOffice.UpdateBy_Time = DateTime.Now;
            airlineOffice.CreateBy = Convert.ToInt32(User.Identity.Name);
            airlineOffice.UpdateBy = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid)
            {
               
                airlineOffice.CompanyNo = Company;
                db.AirlineOffice.Add(airlineOffice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //var country = db.Country01.Where(x => x.Status == 1);
            //ViewBag.Airline_Code = new SelectList(db.Airline, "Airline_Code", "Cname", airlineOffice.Airline.Airline_Code);
            //ViewBag.City_no = new SelectList(db.City03.Where(x => x.Country01.Country_no == airlineOffice.Country01.Country_no),
            //    "City_no", "ShortName", airlineOffice.City03.City_no);
            //ViewBag.Country_no = new SelectList(country, "Country_no", "ShortName", airlineOffice.Country01.Country_no);
            //ViewData["Status"] = GetStuatus.GetStatus(airlineOffice.Status); //取得檔案狀態DropDownList
            getViewData(airlineOffice);
            return View(airlineOffice);
        }

        // GET: AirlineOffices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlineOffice airlineOffice = db.AirlineOffice.Find(id);
            if (airlineOffice == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airlineOffice.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            //ViewData["Status"] = GetStuatus.GetStatus(airlineOffice.Status); //取得檔案狀態DropDownList
            //ViewBag.Airline_Code = airlineOffice.Airline.Airline_Code;
            //ViewBag.City_no = airlineOffice.City03.City_no;
            //ViewBag.Country_no = db.Country01.Where(x => x.Country_no == airlineOffice.Country01.Country_no).FirstOrDefault().Cname;
            getViewData(airlineOffice);
            return View(airlineOffice);
        }

        // POST: AirlineOffices/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,ALoffice_Code,AirlineIndex,AirlineOfficeIndex,CountryIndex,CityIndex,Tele_Order,Office_Number,Office_Fax,Office_Addr,Office_Mailbox,Status,CreateBy,CreateBy_Time,UpdateBy")] AirlineOffice airlineOffice)
        {
            airlineOffice.UpdateBy_Time = DateTime.Now;
            airlineOffice.UpdateBy = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid)
            {             
                db.Entry(airlineOffice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Airline_Code = db.Airline.Where(x => x.Airline_Code == airlineOffice.Airline.Airline_Code).FirstOrDefault().Cname;
            //ViewBag.City_no = db.City03.Where(x => x.City_no == airlineOffice.City03.City_no).FirstOrDefault().Cname;
            //ViewBag.Cuntry_no = db.Country01.Where(x => x.Country_no == airlineOffice.Country01.Country_no).FirstOrDefault().Cname;
            //ViewData["Status"] = GetStuatus.GetStatus(airlineOffice.Status); //取得檔案狀態DropDownList
            getViewData(airlineOffice);
            return View(airlineOffice);
        }
        public void getViewData(AirlineOffice airlineOffice)
        {
            ViewBag.CreateBy = db.HRInfo.Find(airlineOffice.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airlineOffice.UpdateBy).EmpName;
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var airline = db.Airline.Where(x => x.Status == 1).Select(x => new { x.AirlineIndex, Cname = x.Airline_Code + " " + x.Cname });
            ViewBag.AirlineIndex = new SelectList(airline, "AirlineIndex", "Cname",airlineOffice.AirlineIndex);
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", airlineOffice.CountryIndex);
            var city = db.City03.Where(x => x.Country01.CountryIndex == airlineOffice.CountryIndex && x.Status == 1).Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname",airlineOffice.CityIndex);
            ViewData["Status"] = GetStuatus.GetStatus(airlineOffice.Status); //取得檔案狀態DropDownList
        }

        // GET: AirlineOffices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirlineOffice airlineOffice = db.AirlineOffice.Find(id);
            if (airlineOffice == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airlineOffice.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            return View(airlineOffice);
        }

        // POST: AirlineOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            AirlineOffice airlineOffice = db.AirlineOffice.Find(id);
            airlineOffice.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company);
            var NowCity2 = NowCity.Select(x => new { x.CityIndex, x.Cname,x.City_no });
            return Json(NowCity2);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
