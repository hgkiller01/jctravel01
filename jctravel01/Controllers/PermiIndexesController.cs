using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeOurCompny]
    public class PermiIndexesController : Controller
    {
        private TravelContainer db = new TravelContainer();

        // GET: PermiIndexes
        public ActionResult Index()
        {
            return View(db.PermiIndex.ToList());
        }

        // GET: PermiIndexes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermiIndex permiIndex = db.PermiIndex.Find(id);
            if (permiIndex == null)
            {
                return HttpNotFound();
            }
            return View(permiIndex);
        }

        // GET: PermiIndexes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PermiIndexes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Permilindex,MainPerNo,MainPerName,AltPerNo,AltPerName")] PermiIndex permiIndex)
        {
            permiIndex.CreateBy = Convert.ToInt32(User.Identity.Name);
            permiIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
            permiIndex.CreateBy_Time = DateTime.Now;
            permiIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                permiIndex.PermiNo = permiIndex.MainPerName + permiIndex.AltPerNo;
                db.PermiIndex.Add(permiIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(permiIndex);
        }

        // GET: PermiIndexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermiIndex permiIndex = db.PermiIndex.Find(id);
            if (permiIndex == null)
            {
                return HttpNotFound();
            }
            return View(permiIndex);
        }

        // POST: PermiIndexes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Permilindex,PermiNo,MainPerNo,MainPerName,AltPerNo,AltPerName,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] PermiIndex permiIndex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(permiIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(permiIndex);
        }

        // GET: PermiIndexes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PermiIndex permiIndex = db.PermiIndex.Find(id);
            if (permiIndex == null)
            {
                return HttpNotFound();
            }
            return View(permiIndex);
        }

        // POST: PermiIndexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PermiIndex permiIndex = db.PermiIndex.Find(id);
            db.PermiIndex.Remove(permiIndex);
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
