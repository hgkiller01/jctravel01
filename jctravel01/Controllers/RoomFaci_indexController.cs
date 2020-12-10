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
    public class RoomFaci_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: RoomFaci_index
        public ActionResult Index(string Ename, string Cname, string RoomFaci_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowRoomFaci = db.RoomFaci_index.OrderBy(x => x.RoomFaci_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(RoomFaci_code))
            {
                ViewBag.RoomFaci_code = RoomFaci_code;
                nowRoomFaci = nowRoomFaci.Where(x => x.RoomFaci_code.StartsWith(RoomFaci_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowRoomFaci = nowRoomFaci.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowRoomFaci = nowRoomFaci.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowRoomFaci.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowRoomFaci.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: RoomFaci_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomFaci_index roomFaci_index = db.RoomFaci_index.Find(id);
            if (roomFaci_index == null)
            {
                return HttpNotFound();
            }
            return View(roomFaci_index);
        }

        // GET: RoomFaci_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: RoomFaci_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomFaci_no,ShortName,Cname,Ename,Status")] RoomFaci_index roomFaci_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            roomFaci_index.RoomFaci_code = AC.GetAutoCodeRoomFaci();
            roomFaci_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            roomFaci_index.CreateBy_Time = DateTime.Now;
            roomFaci_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            roomFaci_index.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                roomFaci_index.CompanyNo = Company;
                db.RoomFaci_index.Add(roomFaci_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(roomFaci_index.Status);
            return View(roomFaci_index);
        }

        // GET: RoomFaci_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomFaci_index roomFaci_index = db.RoomFaci_index.Find(id);
            if (roomFaci_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (roomFaci_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(roomFaci_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(roomFaci_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(roomFaci_index.Status);
            return View(roomFaci_index);
        }

        // POST: RoomFaci_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomFaci_no,RoomFaci_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] RoomFaci_index roomFaci_index)
        {
            if (ModelState.IsValid)
            {
                if (roomFaci_index.Status == 2)
                {
                    foreach (var item in roomFaci_index.RoomFacility)
                    {
                        item.Main = false;
                    }
                }                
                roomFaci_index.UpdateBy_Time = DateTime.Now;
                roomFaci_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(roomFaci_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(roomFaci_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(roomFaci_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(roomFaci_index.Status);
            return View(roomFaci_index);
        }

        // GET: RoomFaci_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    RoomFaci_index roomFaci_index = db.RoomFaci_index.Find(id);
        //    if (roomFaci_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(roomFaci_index);
        //}

        // POST: RoomFaci_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    RoomFaci_index roomFaci_index = db.RoomFaci_index.Find(id);
        //    db.RoomFaci_index.Remove(roomFaci_index);
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
