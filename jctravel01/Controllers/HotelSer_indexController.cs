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
    public class HotelSer_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: HotelSer_index
        public ActionResult Index(string Ename, string Cname, string Hotel_Ser_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var HotelSer = db.HotelSer_index.OrderBy(x => x.Hotel_Ser_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Hotel_Ser_code))
            {
                ViewBag.Hotel_Ser_code = Hotel_Ser_code;
                HotelSer = HotelSer.Where(x => x.Hotel_Ser_code.StartsWith(Hotel_Ser_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                HotelSer = HotelSer.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                HotelSer = HotelSer.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = HotelSer.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = HotelSer.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: HotelSer_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelSer_index hotelSer_index = db.HotelSer_index.Find(id);
            if (hotelSer_index == null)
            {
                return HttpNotFound();
            }
            return View(hotelSer_index);
        }

        // GET: HotelSer_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: HotelSer_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Hotel_Ser_no,Status,ShortName,Cname,Ename")] HotelSer_index hotelSer_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            hotelSer_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            hotelSer_index.UpdateBy_Time = DateTime.Now;
            hotelSer_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            hotelSer_index.CreateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                hotelSer_index.Hotel_Ser_code = AC.GetAutoCodeHotelSer();
                hotelSer_index.CompanyNo = Company;
                db.HotelSer_index.Add(hotelSer_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(hotelSer_index.Status);
            return View(hotelSer_index);
        }

        // GET: HotelSer_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HotelSer_index hotelSer_index = db.HotelSer_index.Find(id);
            if (hotelSer_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (hotelSer_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(hotelSer_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hotelSer_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(hotelSer_index.Status);
            return View(hotelSer_index);
        }

        // POST: HotelSer_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Hotel_Ser_no,Status,Hotel_Ser_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] HotelSer_index hotelSer_index)
        {
            if (ModelState.IsValid)
            {
                if (hotelSer_index.Status == 2)
                {
                    foreach (var item in hotelSer_index.HotelService)
                    {
                        item.Main = false;
                    }
                }
                hotelSer_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotelSer_index.UpdateBy_Time = DateTime.Now;
                db.Entry(hotelSer_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(hotelSer_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hotelSer_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(hotelSer_index.Status);
            return View(hotelSer_index);
        }

        // GET: HotelSer_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HotelSer_index hotelSer_index = db.HotelSer_index.Find(id);
        //    if (hotelSer_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hotelSer_index);
        //}

        // POST: HotelSer_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    HotelSer_index hotelSer_index = db.HotelSer_index.Find(id);
        //    db.HotelSer_index.Remove(hotelSer_index);
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
