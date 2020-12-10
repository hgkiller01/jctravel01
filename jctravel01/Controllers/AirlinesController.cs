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
    public class AirlinesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: Airlines
        public ActionResult Index(int? Status, string Airline_Code, string Cname, string Ename, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var airline = db.Airline.OrderBy(x => x.Airline_Code).Where(x => x.CompanyNo == Company).Where(x => x.Status == 1 || x.Status == 2 );
            if (!string.IsNullOrEmpty(Airline_Code))
            {
                ViewBag.City_no = Airline_Code;
                airline = airline.Where(x => x.Airline_Code.Contains(Airline_Code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                airline = airline.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                airline = airline.Where(x => x.Ename.Contains(Ename));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                airline = airline.Where(x => x.Status == Status);
            }
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            ViewData["DataCount"] = airline.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = airline.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Airlines/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Airline airline = db.Airline.Find(id);
        //    if (airline == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(airline.Status);
        //    return View(airline);
        //}

        // GET: Airlines/Create
        public ActionResult Create()
        {
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            return View();
        }

        // POST: Airlines/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Airline_Code,AirlineIndex,Cname,ShortName,Ename,Tele_CountryCode,Tele_Area,Tele_number,URL,Fax,Email,Status")] Airline airline)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (!string.IsNullOrEmpty(airline.Airline_Code))
            {
                airline.Airline_Code = airline.Airline_Code.ToUpper();
            }

            int count = db.Airline.Where(x => x.Airline_Code == airline.Airline_Code && x.CompanyNo == Company).Count();
            if (count > 0)
            {
                ModelState.AddModelError("Airline_Code", "公司代碼重複!");
            }
            airline.CreateBy_Time = DateTime.Now;
            airline.UpdateBy_Time = DateTime.Now;
            airline.CreateBy = Convert.ToInt32(User.Identity.Name);
            airline.UpdateBy = Convert.ToInt32(User.Identity.Name);    
            try
            {
                if (ModelState.IsValid)
                {
                    airline.CompanyNo = Company;              
                    db.Airline.Add(airline);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.alert = ex.ToString();
                ViewData["Status"] = GetStuatus.GetStatus(airline.Status); //取得檔案狀態DropDownList
                return View(airline);
            }
            ViewData["Status"] = GetStuatus.GetStatus(airline.Status); //取得檔案狀態DropDownList
            return View(airline);
        }

        // GET: Airlines/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airline airline = db.Airline.Find(id);
            if (airline == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airline.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(airline.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airline.UpdateBy).EmpName;
            ViewData["Status"] = GetStuatus.GetStatus(airline.Status);//取得狀態
            return View(airline);
        }

        // POST: Airlines/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,Airline_Code,AirlineIndex,Cname,ShortName,Ename,Tele_CountryCode,Tele_Area,Tele_number,URL,Fax,Email,Status,CreateBy,CreateBy_Time")] Airline airline)
        {
            string Company = Session["ComnpanyNo"].ToString();
            Relevance Re = new Relevance();
            if (!Re.ValidateStatus(airline, Company))
            {
                ModelState.AddModelError("Status", "請先更改相關資料");
            }
                    
            if (ModelState.IsValid)
            {
                airline.UpdateBy = Convert.ToInt32(User.Identity.Name); 
                airline.UpdateBy_Time = DateTime.Now;
                db.Entry(airline).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Status"] = GetStuatus.GetStatus(airline.Status);
            ViewBag.CreateBy = db.HRInfo.Find(airline.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(airline.UpdateBy).EmpName;
            return View(airline);
        }

        // GET: Airlines/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Airline airline = db.Airline.Find(id);
            if (airline == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (airline.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(airline.Status);
            if (airline.AirlineOffice.Count() > 0)
            {
                ViewBag.ShowDetail = "請先刪除相關資料";
            }
            return View(airline);
        }

        // POST: Airlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Airline airline = db.Airline.Find(id);
            airline.Status = 3;
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
