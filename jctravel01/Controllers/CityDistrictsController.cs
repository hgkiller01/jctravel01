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
    public class CityDistrictsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: CityDistricts
        public ActionResult Index(int? Status, int? CityIndex, int? CountryIndexNum, string DisEname, string DisCname, string CityDistrictCode, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var cityDistrict = db.CityDistrict.OrderBy(x => x.CityDistrictCode).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(CityDistrictCode))
            {
                ViewBag.CityDistrictCode = CityDistrictCode;
                cityDistrict = cityDistrict.Where(x => x.CityDistrictCode.StartsWith(CityDistrictCode));
            }
            if (!string.IsNullOrEmpty(DisCname))
            {
                ViewBag.DisCname = DisCname;
                cityDistrict = cityDistrict.Where(x => x.DisCname.Contains(DisCname));
            }
            if (!string.IsNullOrEmpty(DisEname))
            {
                ViewBag.DisEname = DisEname;
                cityDistrict = cityDistrict.Where(x => x.DisEname.Contains(DisEname));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                cityDistrict = cityDistrict.Where(x => x.Status == Status);
            }
            if (CityIndex != null)
            {
                ViewBag.CityIndex = CityIndex;
                cityDistrict = cityDistrict.Where(x => x.CityIndex == CityIndex);
            }
            ViewData["DataCount"] = cityDistrict.Count();
            ViewBag.RowCountMin = page * pagesize - 4;
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var firstCountry = country.FirstOrDefault();
            if (CountryIndexNum == null)
            {
                ViewBag.CountryIndexNum = new SelectList(country, "CountryIndex", "Cname", firstCountry);
                ViewBag.CityIndexNum = new SelectList("");
            }
            else
            {
                ViewBag.CountryIndexNum = new SelectList(country, "CountryIndex", "Cname", CountryIndexNum);
                var selectCity = db.City03.Where(x => x.CountryIndex == CountryIndexNum && x.Status == 1 && x.CompanyNo == Company)
                    .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
                ViewBag.CityIndexNum = new SelectList(selectCity, "CityIndex","Cname",CityIndex);
            }

            ViewBag.StatusNum = GetStuatus.DefualStatus();
            var result = cityDistrict.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: CityDistricts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDistrict cityDistrict = db.CityDistrict.Find(id);
            if (cityDistrict == null)
            {
                return HttpNotFound();
            }
            return View(cityDistrict);
        }

        // GET: CityDistricts/Create
        public ActionResult Create(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var city = db.City03.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.Status = GetStuatus.GetStatus();
            if (id == null)
            {
                ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname");
            }
            else
            {
                ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname",id);
            }
            return View();
        }

        // POST: CityDistricts/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityDistrictIndex,CityIndex,CityDistrictCode,DisCname,DisEname,Status")] CityDistrict cityDistrict)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            string citycode = db.City03.Find(cityDistrict.CityIndex).City_no;
            cityDistrict.CreateBy = Convert.ToInt32(User.Identity.Name);
            cityDistrict.UpdateBy = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid)
            {
                cityDistrict.CityDistrictCode = Ac.GetAutoCodeArea(citycode);
                cityDistrict.CreateBy_Time = DateTime.Now;
                cityDistrict.UpdateBy_Time = DateTime.Now;
                cityDistrict.CompanyNo = Company;
                db.CityDistrict.Add(cityDistrict);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getViewData(cityDistrict);
            return View(cityDistrict);
        }

        // GET: CityDistricts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDistrict cityDistrict = db.CityDistrict.Find(id);
            if (cityDistrict == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (cityDistrict.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            getViewData(cityDistrict);
            return View(cityDistrict);
        }

        // POST: CityDistricts/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityDistrictIndex,CityIndex,CityDistrictCode,DisCname,DisEname,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] CityDistrict cityDistrict)
        {
            if (ModelState.IsValid)
            {
                cityDistrict.UpdateBy = Convert.ToInt32(User.Identity.Name);
                cityDistrict.UpdateBy_Time = DateTime.Now;
                db.Entry(cityDistrict).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getViewData(cityDistrict);
            var citydistrict2 = db.CityDistrict.Find(cityDistrict.CityDistrictIndex);
            cityDistrict.City03 = citydistrict2.City03;
            return View(cityDistrict);
        }
        public void getViewData(CityDistrict cityDistrict)
        {
            ViewBag.CreateBy = db.HRInfo.Find(cityDistrict.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(cityDistrict.UpdateBy).EmpName;
            string Company = Session["ComnpanyNo"].ToString();
            var city = db.City03.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.Status = GetStuatus.GetStatus(cityDistrict.Status);
            ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname", cityDistrict.CityIndex);
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company);
            var NowCity2 = NowCity.Select(x => new { x.CityIndex, x.City_no, x.Cname });
            return Json(NowCity2);
        }

        // GET: CityDistricts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CityDistrict cityDistrict = db.CityDistrict.Find(id);
            if (cityDistrict == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (cityDistrict.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            return View(cityDistrict);
        }

        // POST: CityDistricts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CityDistrict cityDistrict = db.CityDistrict.Find(id);
            cityDistrict.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
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
