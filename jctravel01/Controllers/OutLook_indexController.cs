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
    public class OutLook_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: OutLook_index
        public ActionResult Index(string Ename, string Cname, string OutLook_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowOutLook = db.OutLook_index.OrderBy(x => x.OutLook_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(OutLook_code))
            {
                ViewBag.OutLook_code = OutLook_code;
                nowOutLook = nowOutLook.Where(x => x.OutLook_code.StartsWith(OutLook_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowOutLook = nowOutLook.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowOutLook = nowOutLook.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowOutLook.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowOutLook.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: OutLook_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutLook_index outLook_index = db.OutLook_index.Find(id);
            if (outLook_index == null)
            {
                return HttpNotFound();
            }
            return View(outLook_index);
        }

        // GET: OutLook_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: OutLook_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OutLook_index1,OutLook_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] OutLook_index outLook_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            outLook_index.OutLook_code = Ac.GetAutoCodeOutLook();
            outLook_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            outLook_index.CreateBy_Time = DateTime.Now;
            outLook_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            outLook_index.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                outLook_index.CompanyNo = Company;
                db.OutLook_index.Add(outLook_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(outLook_index.Status);
            return View(outLook_index);
        }

        // GET: OutLook_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OutLook_index outLook_index = db.OutLook_index.Find(id);
            if (outLook_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (outLook_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(outLook_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(outLook_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(outLook_index.Status);
            return View(outLook_index);
        }

        // POST: OutLook_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OutLook_index1,OutLook_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] OutLook_index outLook_index)
        {
            if (ModelState.IsValid)
            {
                if (outLook_index.Status == 2)
                {
                    foreach (var item in outLook_index.HotelOutLook)
                    {
                        item.Main = false;
                    }
                }
                outLook_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                outLook_index.UpdateBy_Time = DateTime.Now;
                db.Entry(outLook_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(outLook_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(outLook_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(outLook_index.Status);
            return View(outLook_index);
        }

        // GET: OutLook_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    OutLook_index outLook_index = db.OutLook_index.Find(id);
        //    if (outLook_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(outLook_index);
        //}

        // POST: OutLook_index/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OutLook_index outLook_index = db.OutLook_index.Find(id);
            db.OutLook_index.Remove(outLook_index);
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
