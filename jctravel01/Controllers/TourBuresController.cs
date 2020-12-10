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
    public class TourBuresController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private AutoCode AC = new AutoCode();
        private int pagesize = 5;

        // GET: TourBures
        public ActionResult Index(int? Status, int? CountryIndex, string Ename, string Cname, string TourBure_no, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page;
            var tourBure = db.TourBure.OrderBy(x => x.TourBure_no).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(TourBure_no))
            {
                ViewBag.TourBure_no = TourBure_no;
                tourBure = tourBure.Where(x => x.TourBure_no.Contains(TourBure_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                tourBure = tourBure.Where(x => x.Cname.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                tourBure = tourBure.Where(x => x.Ename.Contains(Ename));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                tourBure = tourBure.Where(x => x.Status == Status);
            }
            if (CountryIndex != null)
            {
                ViewBag.CountryIndex = CountryIndex;
                tourBure = tourBure.Where(x => x.CountryIndex == CountryIndex);
            }
            ViewData["DataCount"] = tourBure.Count();
            var allCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndexNum = new SelectList(allCountry, "CountryIndex", "Cname");
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = tourBure.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: TourBures/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TourBure tourBure = db.TourBure.Find(id);
        //    if (tourBure == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tourBure);
        //}

        // GET: TourBures/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new {x.CountryIndex,Cname = x.Country_no+" "+x.Cname });//找出已完成國家
            var firstCountry = country.FirstOrDefault();//選擇第一個已完成國家
            var state = db.State02.Where(x => x.CountryIndex == firstCountry.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new {x.StateIndex,Cname = x.State_no+" "+x.Cname }); //找出所有已經完成的省
            var firstState =state.FirstOrDefault();//選擇第一個已經完成的省           
            if (firstState != null)
            {
                var city = db.City03.Where(x => x.State02.StateIndex == firstState.StateIndex && x.Status == 1)
                    .Select(x => new {x.CityIndex,Cname = x.City_no+" "+x.Cname });//找出相關的城市
                var firstCity = city.FirstOrDefault();//選擇第一個已經完成的城市
                ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname", firstCity.CityIndex);
                ViewBag.StateIndex = new SelectList(state, "StateIndex", "Cname", firstState.StateIndex);
            }
            else
            {
                ViewBag.CityIndex = new SelectList("");
                ViewBag.StateIndex = new SelectList("");
            }
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", firstCountry.CountryIndex);            
            ViewBag.Status = GetStuatus.GetStatus();//顯示完成選項
            return View();
        }

        // POST: TourBures/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryIndex,StateIndex,CityIndex,Cname,Ename,Tele_number,Fax,URL,eMail,Addr,Status")] TourBure tourBure)
        {
            string Company = Session["ComnpanyNo"].ToString();
            string countryCode = db.Country01.Find(tourBure.CountryIndex).Country_no;
            tourBure.CreateBy = Convert.ToInt32(User.Identity.Name);
            tourBure.UpdateBy = Convert.ToInt32(User.Identity.Name);
            tourBure.CreateBy_Time = DateTime.Now;
            tourBure.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                tourBure.CompanyNo = Company;
                tourBure.TourBure_no = AC.GetAutoCodeT(countryCode);
                db.TourBure.Add(tourBure);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });//找出已完成城市
            var state = db.State02.Where(x => x.CountryIndex == tourBure.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.StateIndex, Cname = x.State_no + " " + x.Cname }); //找出所有已經完成的省
            var firstState = state.Where(x => x.StateIndex == tourBure.StateIndex).FirstOrDefault();//目前的第一個相關省
            if (firstState != null)
            {
                var city = db.City03.Where(x => x.State02.StateIndex == firstState.StateIndex && x.Status == 1)
                    .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });//找出相關的城市
                var firstCity = city.FirstOrDefault();//選擇第一個已經完成的城市
                if (firstCity != null)
                {
                    ViewBag.CityIndex = new SelectList(city, "CityIndex", "Cname", firstCity.CityIndex);
                }
                else
                {
                    ViewBag.CityIndex = new SelectList("");
                    ViewBag.StateIndex = new SelectList(state, "StateIndex", "Cname", firstState.StateIndex);
                }
                
                ViewBag.StateIndex = new SelectList(state, "StateIndex", "Cname", firstState.StateIndex);
            }
            else
            {
                ViewBag.CityIndex = new SelectList("");
                ViewBag.StateIndex = new SelectList("");
            }
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", tourBure.CountryIndex);            
            ViewBag.Status = GetStuatus.GetStatus(tourBure.Status);//顯示完成選項
            return View(tourBure);
        }

        // GET: TourBures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourBure tourBure = db.TourBure.Find(id);
            if (tourBure == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (tourBure.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(tourBure.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(tourBure.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(tourBure.Status);//顯示完成選項
            return View(tourBure);
        }

        // POST: TourBures/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TourBureIndex,CompanyNo,TourBure_no,TourBureIndex,CountryIndex,StateIndex,CityIndex,Cname,Ename,Tele_number,Fax,URL,eMail,Addr,Status,CreateBy,CreateBy_Time,UpdateBy")] TourBure tourBure)
        {
            if (ModelState.IsValid)
            {
                tourBure.UpdateBy_Time = DateTime.Now;
                tourBure.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(tourBure).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var tourBure2 = db.TourBure.Find(tourBure.TourBureIndex);
            tourBure.Country01 = tourBure2.Country01;
            tourBure.City03 = tourBure2.City03;
            tourBure.State02 = tourBure2.State02;
            ViewBag.CreateBy = db.HRInfo.Find(tourBure.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(tourBure.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(tourBure.Status);//顯示完成選項
            return View(tourBure);
        }


        // GET: TourBures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TourBure tourBure = db.TourBure.Find(id);
            if (tourBure == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (tourBure.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            return View(tourBure);
        }

        // POST: TourBures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            TourBure tourBure = db.TourBure.Find(id);
            tourBure.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetState_no(int CountryIndex) //以Ajax取得DropDownList目前的省
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowState = db.State02.Where(x => x.CountryIndex == CountryIndex  && x.Status == 1 && x.CompanyNo==Company).Select(x => new { x.StateIndex,x.State_no, x.Cname });

            return Json(NowState);
        }
        [HttpPost]
        public ActionResult GetCity_no(int StateIndex)//以Ajax取得DropDownList目前的城市
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.State02.StateIndex == StateIndex && x.Status == 1 && x.CompanyNo==Company).Select(x => new { x.CityIndex,x.City_no, x.Cname });
            return Json(NowCity);
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
