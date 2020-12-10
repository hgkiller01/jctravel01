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
    public class AmosphIndexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: AmosphIndex
        public ActionResult Index(string Ename, string Cname, string Amosph_no, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowAmosph = db.AmosphIndex.OrderBy(x => x.Amosph_no).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Amosph_no))
            {
                ViewBag.Amosph_no = Amosph_no;
                nowAmosph = nowAmosph.Where(x => x.Amosph_no.StartsWith(Amosph_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowAmosph = nowAmosph.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowAmosph = nowAmosph.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowAmosph.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowAmosph.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: AmosphIndex/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AmosphIndex amosphIndex = db.AmosphIndex.Find(id);
        //    if (amosphIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(amosphIndex);
        //}

        // GET: AmosphIndex/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: AmosphIndex/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Amosph_Index,Status,Amosph_no,ShortName,Cname,Ename")] AmosphIndex amosphIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            amosphIndex.Amosph_no = Ac.GetAutoCodeAmos();
            amosphIndex.CreateBy = Convert.ToInt32(User.Identity.Name);
            amosphIndex.CreateBy_Time = DateTime.Now;
            amosphIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
            amosphIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                amosphIndex.CompanyNo = Company;
                db.AmosphIndex.Add(amosphIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(amosphIndex.Status);
            return View(amosphIndex);
        }

        // GET: AmosphIndex/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AmosphIndex amosphIndex = db.AmosphIndex.Find(id);
            if (amosphIndex == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (amosphIndex.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(amosphIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(amosphIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(amosphIndex.Status);
            return View(amosphIndex);
        }

        // POST: AmosphIndex/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Amosph_Index,Status,Amosph_no,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] AmosphIndex amosphIndex)
        {
            if (ModelState.IsValid)
            {
                if (amosphIndex.Status == 2)
                {
                    if (amosphIndex.ResAmos.Count() > 0)
                    {
                        foreach (var item in amosphIndex.ResAmos)
                        {
                            item.Main = false;
                        }
                    }
                }
                amosphIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                amosphIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(amosphIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(amosphIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(amosphIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(amosphIndex.Status);
            return View(amosphIndex);
        }

        // GET: AmosphIndex/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    AmosphIndex amosphIndex = db.AmosphIndex.Find(id);
        //    if (amosphIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(amosphIndex);
        //}

        // POST: AmosphIndex/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    AmosphIndex amosphIndex = db.AmosphIndex.Find(id);
        //    if (amosphIndex.ResAmos != null)
        //    {
        //        foreach (var item in amosphIndex.ResAmos)
        //        {
        //            item.Main = false;
        //        }
        //    }
        //    db.AmosphIndex.Remove(amosphIndex);
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