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
    public class Type_indexController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: Type_index
        public ActionResult Index(string Ename, string Cname, string Type_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var nowtype = db.Type_index.OrderBy(x => x.Type_code).Where(x => x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Type_code))
            {
                ViewBag.Type_code = Type_code;
                nowtype = nowtype.Where(x => x.Type_code.StartsWith(Type_code));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                nowtype = nowtype.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                nowtype = nowtype.Where(x => x.Ename.Contains(Ename));
            }
            ViewData["DataCount"] = nowtype.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = nowtype.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Type_index/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Type_index type_index = db.Type_index.Find(id);
            if (type_index == null)
            {
                return HttpNotFound();
            }
            return View(type_index);
        }

        // GET: Type_index/Create
        public ActionResult Create()
        {
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: Type_index/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Type_Index1,ShortName,Cname,Ename,Status")] Type_index type_index)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode Ac = new AutoCode();
            if (ModelState.IsValid)
            {
                type_index.Type_code = Ac.GetAutoCodeType();
                type_index.CreateBy = Convert.ToInt32(User.Identity.Name);
                type_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                type_index.CreateBy_Time = DateTime.Now;
                type_index.UpdateBy_Time = DateTime.Now;
                type_index.CompanyNo = Company;
                db.Type_index.Add(type_index);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(type_index.Status);
            return View(type_index);
        }

        // GET: Type_index/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Type_index type_index = db.Type_index.Find(id);
            if (type_index == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (type_index.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(type_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(type_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(type_index.Status);
            return View(type_index);
        }

        // POST: Type_index/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Type_Index1,Status,Type_code,ShortName,Cname,Ename,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] Type_index type_index)
        {
            if (ModelState.IsValid)
            {
                type_index.UpdateBy = Convert.ToInt32(User.Identity.Name);
                type_index.UpdateBy_Time = DateTime.Now;
                if (type_index.Status == 2)
                {
                    if (type_index.Scenery_Type.Count() > 0)
                    {
                        foreach (var item in type_index.Scenery_Type)
                        {
                            item.Main = false;
                        }
                    }
                    if (type_index.Res_Type.Count() > 0)
                    {
                        foreach(var item in type_index.Res_Type)
                        {
                            item.Main = false;
                        }
                    }
                    if (type_index.Hotel_Type.Count() > 0)
                    {
                        foreach (var item in type_index.Hotel_Type)
                        {
                            item.Main = false;
                        }
                    }
                }
                db.Entry(type_index).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(type_index.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(type_index.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(type_index.Status);
            return View(type_index);
        }

        // GET: Type_index/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Type_index type_index = db.Type_index.Find(id);
        //    if (type_index == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(type_index);
        //}

        // POST: Type_index/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Type_index type_index = db.Type_index.Find(id);
        //    db.Type_index.Remove(type_index);
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
