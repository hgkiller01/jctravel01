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
    public class DiningStyIndexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 20;
        // GET: DiningStyIndex
        public ActionResult Index(string Ename, string Cname, string DiningSty_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowDining = db.DiningStyIndex.OrderBy(x => x.DiningSty_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(DiningSty_code))
            {
                ViewBag.DiningSty_code = DiningSty_code;
                nowDining = nowDining.Where(x => x.DiningSty_code.StartsWith(DiningSty_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowDining = nowDining.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowDining = nowDining.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowDining.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 19;
            var result = nowDining.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: DiningStyIndex/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiningStyIndex diningStyIndex = db.DiningStyIndex.Find(id);
            if (diningStyIndex == null)
            {
                return HttpNotFound();
            }
            return View(diningStyIndex);
        }

        // GET: DiningStyIndex/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: DiningStyIndex/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiningSty_Index,Status,DiningSty_code,ShortName,Cname,Ename")] DiningStyIndex diningStyIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            diningStyIndex.DiningSty_code = Ac.GetAutoCodeDining();
            diningStyIndex.CreateBy = Convert.ToInt32(User.Identity.Name);
            diningStyIndex.CreateBy_Time = DateTime.Now;
            diningStyIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
            diningStyIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                diningStyIndex.CompanyNo = Company;
                db.DiningStyIndex.Add(diningStyIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(diningStyIndex.Status);
            return View(diningStyIndex);
        }

        // GET: DiningStyIndex/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiningStyIndex diningStyIndex = db.DiningStyIndex.Find(id);
            if (diningStyIndex == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (diningStyIndex.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(diningStyIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(diningStyIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(diningStyIndex.Status);
            return View(diningStyIndex);
        }

        // POST: DiningStyIndex/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiningSty_Index,Status,DiningSty_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] DiningStyIndex diningStyIndex)
        {
            if (ModelState.IsValid)
            {
                if (diningStyIndex.Status == 2)
                {
                    if (diningStyIndex.ResDining.Count() > 0)
                    {
                        foreach (var item in diningStyIndex.ResDining)
                        {
                            item.Main = false;
                        }
                    }
                }
                diningStyIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                diningStyIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(diningStyIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(diningStyIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(diningStyIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(diningStyIndex.Status);
            return View(diningStyIndex);
        }

        // GET: DiningStyIndex/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DiningStyIndex diningStyIndex = db.DiningStyIndex.Find(id);
        //    if (diningStyIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(diningStyIndex);
        //}

        // POST: DiningStyIndex/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DiningStyIndex diningStyIndex = db.DiningStyIndex.Find(id);
        //    if (diningStyIndex.ResDining != null)
        //    {
        //        foreach (var item in diningStyIndex.ResDining)
        //        {
        //            item.Main = false;
        //        }
        //    }
        //    db.DiningStyIndex.Remove(diningStyIndex);
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
