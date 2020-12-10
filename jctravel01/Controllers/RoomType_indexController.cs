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
    public class RoomType_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: RoomType_index
        public ActionResult Index(string Ename, string Cname, string RoomType_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowRoomType = db.RoomType_index.OrderBy(x => x.RoomType_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(RoomType_code))
            {
                ViewBag.RoomType_code = RoomType_code;
                nowRoomType = nowRoomType.Where(x => x.RoomType_code.StartsWith(RoomType_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowRoomType = nowRoomType.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowRoomType = nowRoomType.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowRoomType.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowRoomType.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: RoomType_index/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoomType_index roomType_index = db.RoomType_index.Find(id);
        //    if (roomType_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roomType_index);
        //}

        // GET: RoomType_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: RoomType_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomType_index1,ShortName,Cname,Ename,Status")] RoomType_index roomType_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            roomType_index.RoomType_code = AC.GetAutoCodeRoomType();
            roomType_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            roomType_index.CreateBy_Time = DateTime.Now;
            roomType_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            roomType_index.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                roomType_index.CompanyNo = Company;
                db.RoomType_index.Add(roomType_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(roomType_index.Status);
            return View(roomType_index);
        }

        // GET: RoomType_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomType_index roomType_index = db.RoomType_index.Find(id);
            if (roomType_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (roomType_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(roomType_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(roomType_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(roomType_index.Status);
            return View(roomType_index);
        }

        // POST: RoomType_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomType_index1,RoomType_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] RoomType_index roomType_index)
        {
            if (ModelState.IsValid)
            {
                roomType_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                roomType_index.UpdateBy_Time = DateTime.Now;
                db.Entry(roomType_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(roomType_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(roomType_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(roomType_index.Status);
            return View(roomType_index);
        }

        // GET: RoomType_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoomType_index roomType_index = db.RoomType_index.Find(id);
        //    if (roomType_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roomType_index);
        //}

        // POST: RoomType_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RoomType_index roomType_index = db.RoomType_index.Find(id);
        //    db.RoomType_index.Remove(roomType_index);
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
