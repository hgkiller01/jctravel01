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
    public class CountryController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5; //每個分頁的資料數量
        // GET: Country
        public ActionResult Index(int? Status, string Ename, string Country_no, string Cname, string Continent, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
                 var Country = db.Country01.OrderBy(x => x.Country_no).
                Where(x => x.CompanyNo == Company).Where(t => t.Status == 1 || t.Status == 2 ).AsNoTracking(); //依國家代號排序,不顯示狀態為3的資料
            
                 if (!string.IsNullOrEmpty(Country_no))//找尋國家
                 {
                     ViewBag.Country = Country_no;
                     Country = Country.Where(x => x.Country_no.StartsWith(Country_no));
                 }
                 if (!string.IsNullOrEmpty(Cname))//找尋國家中文名稱
                 {
                     ViewBag.Cname = Cname;
                     Country = Country.Where(x => x.Cname.Contains(Cname)||x.ShortName.Contains(Cname));
                 }
                 if (!string.IsNullOrEmpty(Continent))//找尋國家洲別
                 {
                     ViewBag.Continent = Continent;
                     Country = Country.Where(x => x.Continent.Contains(Continent));
                 }
                 if (!string.IsNullOrEmpty(Ename))//找尋國家英文名稱
                 {
                     ViewBag.Ename = Ename;
                     Country = Country.Where(x => x.Ename.Contains(Ename));
                 }
                 if (Status != null)//找尋是否完成
                 {
                     ViewBag.Status = Status;
                     Country = Country.Where(x => x.Status == Status);
                 }
            
            MakeContinent Mc = new MakeContinent();
            ViewData["ContinentList"] = Mc.Countinet;
            ViewData["StatusNum"] = GetStuatus.DefualStatus();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            int a = Country.Count();
             ViewData["DataCount"] = Country.Count();
            var result = Country.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }
        // GET: Country/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Country01 country01 = db.Country01.Find(id);
        //    if (country01 == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(country01.Status);//顯示完成狀態
        //    return View(country01);
        //}

        // GET: Country/Create
        public ActionResult Create()
        {
            Plug pg = new Plug();
            ViewBag.Plugcode = pg.plugList;
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            MakeContinent Mc = new MakeContinent();
            ViewData["Continent"] = Mc.Countinet;
            ViewData["PDivisionIndex"] = new SelectList(db.UpDivision.Where(x => x.Status == 1), "PDivisionIndex", "Cname");
            return View();
        }

        // POST: Country/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Country_no,CountryIndex,ShortName,Cname,Ename,Continent,PDivisionIndex,Tele_CountryCode,Tele_DialCode,Voltage,Frequency,Plugcode,Currency_code,Tax,Status,Order")] Country01 country01)
        {
            string Company = Session["ComnpanyNo"].ToString();
            Plug pg = new Plug(country01.Plugcode);
            MakeContinent Mc = new MakeContinent();
            int Person = Convert.ToInt32(User.Identity.Name);
            int countryCout = db.Country01.Where(x => x.Country_no == country01.Country_no && x.CompanyNo == Company).Count();
            if (countryCout > 0)
            {
                ModelState.AddModelError("Country_no", "國家代碼重複");
                getViewData(country01);
                return View(country01);
            }
            country01.CreateBy_Time = DateTime.Now;
            country01.UpdateBy_Time = DateTime.Now;
            country01.CreateBy = Person;
            country01.UpdateBy = Person;
            try { 
            if (ModelState.IsValid)
            {
                country01.CompanyNo = Company;
                country01.Country_no = country01.Country_no.ToUpper();
                db.Country01.Add(country01);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
                }
            catch(Exception ex)
            {
                ViewBag.alert = ex.ToString();
                getViewData(country01);
                return View(country01);
            }

            getViewData(country01);
            return View(country01);
          
        }
        public void getViewData(Country01 country01) //網頁選項傳遞方法
        {
            Plug pg = new Plug(country01.Plugcode);
            MakeContinent Mc = new MakeContinent();
            ViewData["Status"] = GetStuatus.GetStatus(country01.Status); //取得檔案狀態DropDownList               
            ViewBag.Plugcode = pg.plugList;
            ViewBag.CreateBy = db.HRInfo.Find(country01.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(country01.UpdateBy).EmpName;
            ViewData["Continent"] = Mc.MakeNowContinent(country01.Continent);//取得洲名
            ViewData["PDivisionIndex"] = new SelectList(db.UpDivision.Where(x => x.Status == 1), "PDivisionIndex", "Cname", country01.PDivisionIndex);
        }

        // GET: Country/Edit/5
        public ActionResult Edit(int? id)
        {
            MakeContinent Mc = new MakeContinent();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country01 country01 = db.Country01.Find(id);
            if (country01 == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (country01.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(country01.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(country01.UpdateBy).EmpName;
            getViewData(country01);
            return View(country01);
        }

        // POST: Country/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,Country_no,CountryIndex,ShortName,Cname,Ename,Continent,PDivisionIndex,Tele_CountryCode,Tele_DialCode,Voltage,Frequency,Plugcode,Currency_code,Tax,Status,Order,CreateBy,CreateBy_Time,UpdateBy")] Country01 country01)
        {
            string Company = Session["ComnpanyNo"].ToString();
            Relevance Re = new Relevance();
            if (!Re.ValidateStatus(country01, Company)) //驗証是否修改完成狀態時有相關資料
            {
                ModelState.AddModelError("Status", "請先更改相關資料為未完成");
            }
            if (ModelState.IsValid)
            {
                country01.UpdateBy_Time = DateTime.Now;
                country01.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(country01).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getViewData(country01);
            return View(country01);
        }

        // GET: Country/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country01 country01 = db.Country01.Find(id);
            if (country01 == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (country01.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(country01.Status);//取得檔案狀態
            int allCount = country01.City03.Count() + country01.State02.Count()
                +country01.AirlineOffice.Count()+country01.Restaurant.Count()+country01.Scenery.Count();
            if (allCount > 0)
            {
             ViewBag.ShowDetail = "請先刪除相關資料";
            }
           
            return View(country01);
        }

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country01 country01 = db.Country01.Find(id);
            country01.Status = 3;
            db.Entry(country01).State = EntityState.Modified;
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

