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
    public class Theme_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;

        // GET: Theme_index
        public ActionResult Index(string Ename, string Cname, string Theme_Code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var theme = db.Theme_index.OrderBy(x => x.Theme_Code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Theme_Code))
            {
                ViewBag.Theme_Code = Theme_Code;
                theme = theme.Where(x => x.Theme_Code.StartsWith(Theme_Code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                theme = theme.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                theme = theme.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = theme.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = theme.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Theme_index/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Theme_index theme_index = db.Theme_index.Find(id);
        //    if (theme_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(theme_index);
        //}

        // GET: Theme_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: Theme_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Theme_Index1,ShortName,Status,Cname,Ename")] Theme_index theme_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            theme_index.CreateBy = Convert.ToInt32(User.Identity.Name);
            theme_index.CreateBy_Time = DateTime.Now;
            theme_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
            theme_index.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                theme_index.Theme_Code = Ac.GetAutoCodeTheme();
                theme_index.CompanyNo = Company;
                db.Theme_index.Add(theme_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(theme_index.Status);
            return View(theme_index);
        }

        // GET: Theme_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Theme_index theme_index = db.Theme_index.Find(id);
            if (theme_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (theme_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(theme_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(theme_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(theme_index.Status);
            return View(theme_index);
        }

        // POST: Theme_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Theme_Index1,Status,Theme_Code,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] Theme_index theme_index)
        {
            if (ModelState.IsValid)
            {
                theme_index.UpdateBy_Time = DateTime.Now;
                theme_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                if (theme_index.Status == 2)
                {
                    if (theme_index.Scenery_Theme.Count() > 0)
                    {
                        foreach (var item in theme_index.Scenery_Theme)
                        {
                            item.Main = false;
                        }
                    }
                    if (theme_index.Res_Theme.Count() > 0)
                    {
                        foreach (var item in theme_index.Res_Theme)
                        {
                            item.Main = false;
                        }
                    }
                    if (theme_index.Hotel_Theme.Count() > 0)
                    {
                        foreach (var item in theme_index.Hotel_Theme)
                        {
                            item.Main = false;
                        }
                    }
                }
                db.Entry(theme_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(theme_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(theme_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(theme_index.Status);
            return View(theme_index);
        }

        // GET: Theme_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Theme_index theme_index = db.Theme_index.Find(id);
        //    if (theme_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(theme_index);
        //}

        //// POST: Theme_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Theme_index theme_index = db.Theme_index.Find(id);
        //    if (theme_index.Scenery_Theme != null)
        //    {
        //        foreach (var item in theme_index.Scenery_Theme)
        //        {
        //            item.Main = false;
        //        }
        //    }
        //    if (theme_index.Res_Theme != null)
        //    {
        //        foreach(var item in theme_index.Res_Theme)
        //        {
        //            item.Main = false;
        //        }
        //    }

        //    db.Theme_index.Remove(theme_index);
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
