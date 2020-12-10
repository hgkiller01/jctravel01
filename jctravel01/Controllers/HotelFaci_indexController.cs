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
    public class HotelFaci_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: HotelFaci_index
        public ActionResult Index(string Ename, string Cname, string HotelFaci_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var HotelFaci = db.HotelFaci_index.OrderBy(x => x.HotelFaci_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(HotelFaci_code))
            {
                ViewBag.HotelFaci_code = HotelFaci_code;
                HotelFaci = HotelFaci.Where(x => x.HotelFaci_code.StartsWith(HotelFaci_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                HotelFaci = HotelFaci.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                HotelFaci = HotelFaci.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = HotelFaci.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = HotelFaci.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: HotelFaci_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelFaci_index hotelFaci_index = db.HotelFaci_index.Find(id);
            if (hotelFaci_index == null)
            {
                return HttpNotFound();
            }
            return View(hotelFaci_index);
        }

        // GET: HotelFaci_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: HotelFaci_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ShortName,Status,Cname,Ename")] HotelFaci_index hotelFaci_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            hotelFaci_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            hotelFaci_index.UpdateBy_Time = DateTime.Now;
            hotelFaci_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            hotelFaci_index.CreateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                hotelFaci_index.HotelFaci_code = Ac.GetAutoCodeHotelFaci();
                hotelFaci_index.CompanyNo = Company;
                db.HotelFaci_index.Add(hotelFaci_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(hotelFaci_index.Status);
            return View(hotelFaci_index);
        }

        // GET: HotelFaci_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelFaci_index hotelFaci_index = db.HotelFaci_index.Find(id);
            if (hotelFaci_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (hotelFaci_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(hotelFaci_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hotelFaci_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(hotelFaci_index.Status);
            return View(hotelFaci_index);
        }

        // POST: HotelFaci_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "HotelFaci_no,Status,HotelFaci_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] HotelFaci_index hotelFaci_index)
        {
            if (ModelState.IsValid)
            {
                if (hotelFaci_index.Status == 2)
                {
                    foreach(var item in hotelFaci_index.HotelFacility)
                    {
                        item.Main = false;
                    }
                }
                hotelFaci_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotelFaci_index.UpdateBy_Time = DateTime.Now;
                db.Entry(hotelFaci_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(hotelFaci_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hotelFaci_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(hotelFaci_index.Status);
            return View(hotelFaci_index);
        }

        // GET: HotelFaci_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HotelFaci_index hotelFaci_index = db.HotelFaci_index.Find(id);
        //    if (hotelFaci_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hotelFaci_index);
        //}

        // POST: HotelFaci_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    HotelFaci_index hotelFaci_index = db.HotelFaci_index.Find(id);
        //    db.HotelFaci_index.Remove(hotelFaci_index);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
