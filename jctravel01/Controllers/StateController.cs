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
    public class StateController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: State
        public ActionResult Index(string State_no, string Ename, string Cname, int? CountryIndex, int? Status, int page = 1)
        {
            int CurrentPage = page < 1 ? 1 : page;
            string Company = Session["ComnpanyNo"].ToString();
            var state02 = db.State02.OrderBy(x => x.State_no).Where(x => x.CompanyNo == Company)
               .Where(x => x.Status == 1 || x.Status == 2).AsNoTracking();
            if (!string.IsNullOrEmpty(State_no))
            {
                ViewBag.State_no = State_no;
                state02 = state02.Where(x => x.State_no.StartsWith(State_no));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                state02 = state02.Where(x => x.Ename.Contains(Ename) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                state02 = state02.Where(x => x.Cname.Contains(Cname));
            }
            if(CountryIndex != null)
            {
                ViewBag.CountryIndex = CountryIndex;
                state02 = state02.Where(x => x.CountryIndex == CountryIndex);
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                state02 = state02.Where(x => x.Status == Status);
            }
            ViewData["StatusNum"] = GetStuatus.DefualStatus();
            ViewData["DataCount"] = state02.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewData["CountryIndexList"] = new SelectList(ListCountry, "CountryIndex","Cname");
            var result = state02.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }
        // GET: State/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    State02 state02 = db.State02.Find(id);
        //    if (state02 == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStatus = GetStuatus.ValidaStatus(state02.Status);
        //    return View(state02);
        //}

        // GET: State/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndex = new SelectList(ListCountry, "CountryIndex", "Cname");
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: State/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "State_no,CountryIndex,StateIndex,ShortName,Cname,Ename,Status")] State02 state02)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            if (db.State02.Where(x => x.State_no == state02.State_no && x.CompanyNo == Company).Count() > 0)
            {
                ModelState.AddModelError("State_no", "洲/省代碼重複");
            }
            state02.CreateBy_Time = DateTime.Now;
            state02.UpdateBy_Time = DateTime.Now;
            state02.CreateBy = Convert.ToInt32(User.Identity.Name);
            state02.UpdateBy = Convert.ToInt32(User.Identity.Name);
                if (ModelState.IsValid)
                {
                    state02.CompanyNo = Company;
                    state02.State_no = state02.State_no.ToUpper();
                    db.State02.Add(state02);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            ViewBag.CountryIndex = new SelectList(ListCountry, "CountryIndex", "Cname", state02.CountryIndex);
            ViewBag.Status = GetStuatus.GetStatus(state02.Status);
            return View(state02);
        }

        // GET: State/Edit/5
        
        public ActionResult Edit(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State02 state02 = db.State02.Find(id);
            if (state02 == null)
            {
                return HttpNotFound();
            }
            if (state02.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(state02.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(state02.UpdateBy).EmpName;
            ViewBag.CountryIndex = new SelectList(ListCountry, "CountryIndex", "Cname", state02.CountryIndex);
            ViewBag.Status = GetStuatus.GetStatus(state02.Status);
            return View(state02);
        }

        // POST: State/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,State_no,CountryIndex,StateIndex,ShortName,Cname,Ename,Status,CreateBy,CreateBy_Time,UpdateBy")] State02 state02)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var ListCountry = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            Relevance Re = new Relevance();
            if (!Re.ValidateStatus(state02,Company))
            {
                ModelState.AddModelError("Status", "請先更改相關資料");
            }
            if (ModelState.IsValid)
            {
                state02.UpdateBy_Time = DateTime.Now;
                state02.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(state02).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(state02.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(state02.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(state02.Status);
            ViewBag.CountryIndex = new SelectList(ListCountry, "CountryIndex", "Cname", state02.CountryIndex);
            return View(state02);
        }

        // GET: State/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            State02 state02 = db.State02.Find(id);
            if (state02 == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (state02.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStatus = GetStuatus.ValidaStatus(state02.Status);
            int count = state02.City03.Count() + state02.TourBure.Count();
            if (count > 0)
            {
                ViewBag.ShowDetail = "請先刪除相關資料";
            }
            
            return View(state02);
        }

        // POST: State/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            State02 state02 = db.State02.Find(id);
            state02.Status = 3;
            db.Entry(state02).State = EntityState.Modified;
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
