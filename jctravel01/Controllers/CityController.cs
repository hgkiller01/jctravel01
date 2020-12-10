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
    public class CityController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        private TakeUTC Tu = new TakeUTC();
        private TakeAirPort Ta = new TakeAirPort();
        // GET: City
        public ActionResult Index(int? Status, int? CountryIndex,string City_no , string Cname , string Ename ,int page =1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            var city03 = db.City03.OrderBy(x => x.City_no).Where(x => x.CompanyNo == Company).Where(x => x.Status == 1 || x.Status == 2 ).AsNoTracking();
            if (!string.IsNullOrEmpty(City_no))
            {
                ViewBag.City_no = City_no;
               city03 = city03.Where(x => x.City_no.Contains(City_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                city03 = city03.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
               city03 = city03.Where(x => x.Ename.Contains(Ename));
            }
            if (CountryIndex != null)
            {
                ViewBag.CountryIndex = CountryIndex;
                city03 = city03.Where(x => x.CountryIndex == CountryIndex);
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
               city03 =  city03.Where(x => x.Status == Status);
            }
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewData["CountryIndexList"] = new SelectList(ListCountry, "CountryIndex","Cname");
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            ViewData["DataCount"] = city03.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = city03.ToPagedList(CurrentPage, pagesize);//進行分頁
            return View(result);
        }

        // GET: City/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    City03 city03 = db.City03.Find(id);
        //    if (city03 == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(city03.Status);
        //    return View(city03);
        //}

        // GET: City/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            ViewBag.UTC = Tu.UTCSelect;
            var allDisplayCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname }); //所有已完成的城市資料
            var selectCountry = allDisplayCountry.FirstOrDefault(); //一開始選擇的城市
            ViewBag.CountryIndex = new SelectList(allDisplayCountry, "CountryIndex", "Cname", selectCountry.CountryIndex); //取得城市SelectList         
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            var allDisplayState = db.State02.Where(x => x.Status == 1 && x.CompanyNo == Company && x.Country01.CountryIndex == selectCountry.CountryIndex).Select(x => new { x.StateIndex, Cname = x.State_no + " " + x.Cname });
            ViewBag.StateIndex = new SelectList(allDisplayState, "StateIndex", "Cname"); //取得省SelectList
            ViewBag.Airport_type = Ta.AirportList;//取得機場類型
            return View();
        }

        // POST: City/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "City_no,CityIndex,CountryIndex,StateIndex,ShortName,Cname,Ename,Tele_Area,Airport_type,UTC,Daylight_DateTime,Order,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] City03 city03)
        {
            //var allDisplayCountry = db.Country01.Where(x => x.Status == 1); //所有已完成的城市資料
            //var selectCountry = allDisplayCountry.Where(x => x.Country_no == city03.Country01.Country_no).First(); //選擇的城市
            string Company = Session["ComnpanyNo"].ToString();
            int count = db.City03.Where(x => x.City_no == city03.City_no && x.CompanyNo == Company).Count();
            if (count > 0)
            {
                ModelState.AddModelError("City_no", "城市代碼重複");
            }
            city03.CreateBy = Convert.ToInt32(User.Identity.Name);
            city03.UpdateBy = Convert.ToInt32(User.Identity.Name);
            try
            {
                if (ModelState.IsValid)
                {

                    city03.CompanyNo = Company;
                    city03.City_no = city03.City_no.ToUpper();
                    city03.CreateBy_Time = DateTime.Now;
                    city03.UpdateBy_Time = DateTime.Now;
                    db.City03.Add(city03);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                getViewData(city03);
                return View(city03);
            }
            getViewData(city03);
            return View(city03);
        }
        public void getViewData(City03 city03)
        {
            string Company = Session["ComnpanyNo"].ToString();
            ViewBag.CreateBy = db.HRInfo.Find(city03.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(city03.UpdateBy).EmpName;
            var allDisplayCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname }); //所有已完成的城市資料
            ViewBag.CountryIndex = new SelectList(allDisplayCountry, "CountryIndex", "Cname", city03.CountryIndex); //取得城市SelectList         
            ViewData["Status"] = GetStuatus.GetStatus(city03.Status); //取得檔案狀態DropDownList
            var selectCountry = allDisplayCountry.Where(x => x.CountryIndex == city03.CountryIndex).First(); //選擇的城市
            var allDisplayState = db.State02.Where(x => x.Status == 1 && x.CompanyNo == Company && x.Country01.CountryIndex == city03.CountryIndex)
                .Select(x => new { x.StateIndex, Cname = x.State_no + " " + x.Cname });
            ViewBag.StateIndex = new SelectList(allDisplayState, "StateIndex", "Cname",city03.StateIndex); //取得省SelectList
            Tu.SetNowSelect(city03.UTC);//設定UTC
            ViewBag.UTC = Tu.UTCSelect;
            Ta.SetNowSelect(city03.Airport_type);//設定機場
            ViewBag.Airport_type = Ta.AirportList;
        }

        // GET: City/Edit/5
        public ActionResult Edit(int? id)
        {
  
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City03 city03 = db.City03.Find(id);
            if (city03 == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (city03.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            getViewData(city03);
            return View(city03);
        }

        // POST: City/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,City_no,CityIndex,CountryIndex,StateIndex,ShortName,Cname,Ename,Tele_Area,Airport_type,UTC,Daylight_DateTime,Order,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] City03 city03)
        {
            string Company = Session["ComnpanyNo"].ToString();
            Relevance Re = new Relevance();
            if (!Re.ValidateStatus(city03, Company))
            {
                getViewData(city03);
                ViewBag.alert = "因為有相關資料，所以無法更改狀態";
                return View(city03);
            }
            if (ModelState.IsValid)
            {
                city03.UpdateBy = Convert.ToInt32(User.Identity.Name);
                city03.UpdateBy_Time = DateTime.Now;
                db.Entry(city03).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getViewData(city03);
            return View(city03);
        }

        // GET: City/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City03 city03 = db.City03.Find(id);
            if (city03 == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (city03.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            if(city03.AirlineOffice.Count()>0 || city03.Scenery.Count()>0 || city03.Restaurant.Count()>0
                ||city03.Hotel.Count()>0||city03.AirlineOffice.Count()>0||city03.AirportInfo.Count()>0)
            {
                ViewBag.ShowDetail = "請先將相關資料刪除";
            }
            return View(city03);
        }

        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            City03 city03 = db.City03.Find(id);
            city03.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetState_no(int CountryIndex) //以Ajax取得DropDownList目前的國家
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowState = db.State02.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.State_no, x.StateIndex, x.Cname });

            return Json(NowState);
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
