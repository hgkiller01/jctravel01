using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;
using PagedList;
using jctravel01.Models.ViewModel;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeCompny]
    public class AirportInfoesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5; //每個分頁的資料數量
        // GET: AirportInfoes
        public ActionResult Index(int? Status, int? CountryIndex, string ApEName, string ApCname, string AirportCode, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var airportInfo = db.AirportInfo.OrderBy(x => x.AirportCode).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company).AsNoTracking();
            if (!string.IsNullOrEmpty(AirportCode))
            {
                ViewBag.AirportCode = AirportCode;
                airportInfo = airportInfo.Where(x => x.AirportCode.StartsWith(AirportCode));
            }
            if (!string.IsNullOrEmpty(ApCname))
            {
                ViewBag.ApCname = ApCname;
                airportInfo = airportInfo.Where(x => x.ApCname.Contains(ApCname));
            }
            if (!string.IsNullOrEmpty(ApEName))
            {
                ViewBag.ApEName = ApEName;
                airportInfo = airportInfo.Where(x => x.ApEName.Contains(ApEName));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                airportInfo = airportInfo.Where(x => x.Status == Status);
            }
            if (CountryIndex != null)
            {
                ViewBag.CountryIndex = CountryIndex;
                airportInfo = airportInfo.Where(x => x.CountryIndex == CountryIndex);
            }
            ViewData["DataCount"] = airportInfo.Count();
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewData["CountryIndexList"] = new SelectList(ListCountry, "CountryIndex", "Cname");
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = airportInfo.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: AirportInfoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirportInfo airportInfo = db.AirportInfo.Find(id);
            if (airportInfo == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(airportInfo.Status);//取得檔案狀態
            return View(airportInfo);
        }

        // GET: AirportInfoes/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var firstCountry = country.First();
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", firstCountry);
            var selectCity = db.City03.Where(x => x.CountryIndex == firstCountry.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname");
            ViewBag.Status = GetStuatus.GetStatus();//顯示完成選項
            return View();
        }

        // POST: AirportInfoes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AirportCode,AirportIndex,CountryIndex,CityIndex,ApEName,ApCname,Status")] AirportInfo airportInfo)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int AirportCout = db.AirportInfo.Where(x => x.AirportCode == airportInfo.AirportCode && x.CompanyNo == Company).Count();
            if (AirportCout > 0)
            {
                ModelState.AddModelError("AirportCode", "機場代碼重複");
            }
            airportInfo.CreateBy = Convert.ToInt32(User.Identity.Name);
            airportInfo.CreateBy_Time = DateTime.Now;
            airportInfo.UpdateBy = Convert.ToInt32(User.Identity.Name);
            airportInfo.UpdateBy_Time = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    airportInfo.AirportCode = airportInfo.AirportCode.ToUpper();
                    airportInfo.CompanyNo = Company;
                    db.AirportInfo.Add(airportInfo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                getViewdata(airportInfo);
                return View(airportInfo);
            }
            getViewdata(airportInfo);
            return View(airportInfo);
        }

        // GET: AirportInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirportInfo airportInfo = db.AirportInfo.Find(id);
            if (airportInfo == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airportInfo.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.Status = GetStuatus.GetStatus(airportInfo.Status);//顯示選擇狀態
            ViewBag.CreateBy = db.HRInfo.Find(airportInfo.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airportInfo.UpdateBy).EmpName;
            return View(airportInfo);
        }

        // POST: AirportInfoes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AirportCode,AirportIndex,CountryIndex,CityIndex,ApEName,ApCname,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] AirportInfo airportInfo)
        {
            if (ModelState.IsValid)
            {
                airportInfo.UpdateBy_Time = DateTime.Now;
                airportInfo.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(airportInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var airport2 = db.AirportInfo.Find(airportInfo.AirportIndex);
            airportInfo.Country01 = airport2.Country01;
            airportInfo.City03 = airport2.City03;
            ViewBag.CreateBy = db.HRInfo.Find(airportInfo.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airportInfo.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(airportInfo.Status);//顯示選擇狀態
           
            return View(airportInfo);
        }
        public void getViewdata(AirportInfo airportInfo)
        {
            string Company = Session["ComnpanyNo"].ToString();
            ViewBag.CreateBy = db.HRInfo.Find(airportInfo.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airportInfo.UpdateBy).EmpName;
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", airportInfo.CountryIndex);
            var selectCity = db.City03.Where(x => x.CountryIndex == airportInfo.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname", airportInfo.CityIndex);
            ViewBag.Status = GetStuatus.GetStatus(airportInfo.Status);
        }

        // GET: AirportInfoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AirportInfo airportInfo = db.AirportInfo.Find(id);
            if (airportInfo == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airportInfo.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(airportInfo.Status);//取得檔案狀態
            return View(airportInfo);
        }

        // POST: AirportInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            AirportInfo airportInfo = db.AirportInfo.Find(id);
            airportInfo.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)//以Ajax取得城市資料
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company);
            var NowCity2 = NowCity.Select(x => new { x.CityIndex, x.Cname, x.City_no });
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
