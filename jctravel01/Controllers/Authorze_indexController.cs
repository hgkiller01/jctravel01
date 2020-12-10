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
    public class Authorze_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();

        // GET: Authorze_index
        public ActionResult Index()
        {
            var authorze_index = db.Authorze_index.Include(a => a.PermiIndex1);
            return View(authorze_index.ToList());
        }

        // GET: Authorze_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorze_index authorze_index = db.Authorze_index.Find(id);
            if (authorze_index == null)
            {
                return HttpNotFound();
            }
            return View(authorze_index);
        }

        // GET: Authorze_index/Create
        public ActionResult Create()
        {
            Dictionary<int,string> permitList  = new Dictionary<int,string>();
            permitList.Add(1,"讀取");
            permitList.Add(2,"修改");
            permitList.Add(3,"共用");
            ViewBag.Permit = new SelectList(permitList, "key", "value");
            ViewBag.PermiIndex = new SelectList(db.PermiIndex, "Permilindex", "AltPerName");
            return View();
        }

        // POST: Authorze_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AutIndex,PermiIndex,Permit")] Authorze_index authorze_index)
        {
            var repeat = db.Authorze_index.Where(x => x.PermiIndex == authorze_index.PermiIndex && x.Permit == authorze_index.Permit);
            if (repeat.Count() > 0)
            {
                ModelState.AddModelError("Permit", "重複權限");
            }
            if (ModelState.IsValid)
            {
                authorze_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                authorze_index.CreateBy = Convert.ToInt32(User.Identity.Name);
                authorze_index.UpdateBy_Time = DateTime.Now;
                authorze_index.CreateBy_Time = DateTime.Now;
                db.Authorze_index.Add(authorze_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Dictionary<int, string> permitList = new Dictionary<int, string>();
            permitList.Add(1, "讀取");
            permitList.Add(2, "修改");
            permitList.Add(3, "共用");
            ViewBag.Permit = new SelectList(permitList, "key", "value");
            ViewBag.PermiIndex = new SelectList(db.PermiIndex, "Permilindex", "AltPerName", authorze_index.PermiIndex);
            return View(authorze_index);
        }

        // GET: Authorze_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorze_index authorze_index = db.Authorze_index.Find(id);
            if (authorze_index == null)
            {
                return HttpNotFound();
            }
            ViewBag.PermiIndex = new SelectList(db.PermiIndex, "Permilindex", "PermiNo", authorze_index.PermiIndex);
            return View(authorze_index);
        }

        // POST: Authorze_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AutIndex,PermiIndex,Permit,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] Authorze_index authorze_index)
        {
            if (ModelState.IsValid)
            {
                db.Entry(authorze_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PermiIndex = new SelectList(db.PermiIndex, "Permilindex", "PermiNo", authorze_index.PermiIndex);
            return View(authorze_index);
        }

        // GET: Authorze_index/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Authorze_index authorze_index = db.Authorze_index.Find(id);
            if (authorze_index == null)
            {
                return HttpNotFound();
            }
            return View(authorze_index);
        }

        // POST: Authorze_index/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Authorze_index authorze_index = db.Authorze_index.Find(id);
            db.Authorze_index.Remove(authorze_index);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult CreateAll()
        {
            var all = db.PermiIndex.ToList();
            foreach (var item in all)
            {
                for (int i = 1; i < 4; i++)
                {
                    var repeat = db.Authorze_index.Where(x => x.PermiIndex == item.Permilindex && x.Permit == i);
                    if (repeat.Count() == 0)
                    {
                        db.Authorze_index.Add(new Authorze_index()
                        {
                            Permit = i,
                            PermiIndex = item.Permilindex,
                            UpdateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy_Time = DateTime.Now,
                            UpdateBy_Time = DateTime.Now,
                        });
                    }
                }
            }
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
