using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;
using PagedList;
using jctravel01.Models.ViewModel;

namespace jctravel01.Controllers
{
    [AuthorizeCompny]
    [Authorize]
    public class DepIndexesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: DepIndexes
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            var Dep = db.DepIndex.OrderBy(x => x.DepNo).Where(x => x.CompanyNo == CompanyNo);
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            if (Select != null)
            {
                if (Select == 1)
                {
                    Dep = Dep.Where(x => x.DepNo.StartsWith(Search));
                }
                else
                {
                    Dep = Dep.Where(x => x.DepName.Contains(Search));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    Dep = Dep.Where(x => x.DepNo.StartsWith(Search) || x.DepName.Contains(Search));
                }
            }
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            Dictionary<int, string> searchList = new Dictionary<int, string>();
            searchList.Add(1, "部門代號");
            searchList.Add(2, "部門名稱");
            ViewBag.SelectBar = new SelectList(searchList, "key", "value");
            ViewData["DataCount"] = Dep.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = Dep.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: DepIndexes/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DepIndex depIndex = db.DepIndex.Find(id);
        //    if (depIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(depIndex);
        //}

        // GET: DepIndexes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DepIndexes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Dep_Index,DepNo,DepName")] DepIndex depIndex)
        {
            AutoCode AC = new AutoCode();
            string CompanyNo = Session["ComnpanyNo"].ToString();
            int Person = Convert.ToInt32(User.Identity.Name);
            depIndex.CreateBy = Person;
            depIndex.CreateBy_Time = DateTime.Now;
            depIndex.UpdateBy = Person;
            depIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                depIndex.DepNo = AC.GetAutoCodeDep(CompanyNo);
                depIndex.CompanyNo = CompanyNo;
                depIndex.Status = 1;
                db.DepIndex.Add(depIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(depIndex);
        }

        // GET: DepIndexes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DepIndex depIndex = db.DepIndex.Find(id);
            if (depIndex == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (depIndex.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(depIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(depIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(depIndex.Status);
            
            return View(depIndex);
        }

        // POST: DepIndexes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Dep_Index,CompanyNo,Status,DepNo,DepName,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] DepIndex depIndex)
        {
          var hrInfo = db.HRInfo.Where(x => x.Dep_Index == depIndex.Dep_Index);
            if (depIndex.Status == 2)
            {
                if (hrInfo.Count() > 0)
                {
                    foreach (var item in hrInfo)
                    {
                        if (item.OnJobStatus != 4)
                        {
                            ModelState.AddModelError("Status", "請先將此部門人員調離或更改為離職狀態");
                            break;
                        }
                    }
                }
            }
            
            if (ModelState.IsValid)
            {
                depIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                depIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(depIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = GetStuatus.GetStatus(depIndex.Status);
            ViewBag.CreateBy = db.HRInfo.Find(depIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(depIndex.UpdateBy).EmpName;
            return View(depIndex);
        }

        // GET: DepIndexes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DepIndex depIndex = db.DepIndex.Find(id);
        //    if (depIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(depIndex);
        //}

        // POST: DepIndexes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    DepIndex depIndex = db.DepIndex.Find(id);
        //    db.DepIndex.Remove(depIndex);
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
