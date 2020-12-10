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
    public class NearbyFcai_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: NearbyFcai_index
        public ActionResult Index(string Ename, string Cname, string NearbyFaci_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowNearbyFaci = db.NearbyFcai_index.OrderBy(x => x.NearbyFaci_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(NearbyFaci_code))
            {
                ViewBag.NearbyFaci_code = NearbyFaci_code;
                nowNearbyFaci = nowNearbyFaci.Where(x => x.NearbyFaci_code.StartsWith(NearbyFaci_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowNearbyFaci = nowNearbyFaci.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowNearbyFaci = nowNearbyFaci.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowNearbyFaci.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowNearbyFaci.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: NearbyFcai_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NearbyFcai_index nearbyFcai_index = db.NearbyFcai_index.Find(id);
            if (nearbyFcai_index == null)
            {
                return HttpNotFound();
            }
            return View(nearbyFcai_index);
        }

        // GET: NearbyFcai_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: NearbyFcai_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NearbyFaci_no,Status,ShortName,Cname,Ename")] NearbyFcai_index nearbyFcai_index)
        {
            AutoCode AC = new AutoCode();
            nearbyFcai_index.NearbyFaci_code = AC.GetAutoCodeNearFaci();
            string Company = Session["ComnpanyNo"].ToString();
            nearbyFcai_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            nearbyFcai_index.CreateBy_Time = DateTime.Now;
            nearbyFcai_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            nearbyFcai_index.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                nearbyFcai_index.CompanyNo = Company;
                db.NearbyFcai_index.Add(nearbyFcai_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(nearbyFcai_index.Status);
            return View(nearbyFcai_index);
        }

        // GET: NearbyFcai_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NearbyFcai_index nearbyFcai_index = db.NearbyFcai_index.Find(id);
            if (nearbyFcai_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (nearbyFcai_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(nearbyFcai_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(nearbyFcai_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(nearbyFcai_index.Status);
            return View(nearbyFcai_index);
        }

        // POST: NearbyFcai_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NearbyFaci_no,Status,NearbyFaci_code,CompanyNo,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] NearbyFcai_index nearbyFcai_index)
        {
            if (ModelState.IsValid)
            {
                if (nearbyFcai_index.Status == 2)
                {
                    foreach (var item in nearbyFcai_index.NearbyFacility)
                    {
                        item.Main = false;
                    }
                }
                nearbyFcai_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                nearbyFcai_index.UpdateBy_Time = DateTime.Now;
                db.Entry(nearbyFcai_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(nearbyFcai_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(nearbyFcai_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(nearbyFcai_index.Status);
            return View(nearbyFcai_index);
        }

        // GET: NearbyFcai_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    NearbyFcai_index nearbyFcai_index = db.NearbyFcai_index.Find(id);
        //    if (nearbyFcai_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(nearbyFcai_index);
        //}

        // POST: NearbyFcai_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    NearbyFcai_index nearbyFcai_index = db.NearbyFcai_index.Find(id);
        //    db.NearbyFcai_index.Remove(nearbyFcai_index);
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
