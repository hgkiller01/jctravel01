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
    [AuthorizeCompny]
    [Authorize]
    public class JobGroupController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: JobGroup
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            var JobG = db.JobGruopIndex.OrderBy(x => x.JobGruopNo).Where(x => x.CompanyNo == CompanyNo);
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            if (Select != null)
            {
                if (Select == 1)
                {
                    JobG = JobG.Where(x => x.JobGruopNo.StartsWith(Search));
                }
                else
                {
                    JobG = JobG.Where(x => x.JobGroupName.Contains(Search));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    JobG = JobG.Where(x => x.JobGruopNo.StartsWith(Search) || x.JobGroupName.Contains(Search));
                }
            }
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            Dictionary<int, string> searchList = new Dictionary<int, string>();
            searchList.Add(1, "群組代號");
            searchList.Add(2, "群組名稱");
            ViewBag.SelectBar = new SelectList(searchList, "key", "value");
            ViewData["DataCount"] = JobG.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = JobG.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: JobGroup/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobGruopIndex jobGruopIndex = db.JobGruopIndex.Find(id);
        //    if (jobGruopIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobGruopIndex);
        //}

        // GET: JobGroup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobGroup/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobGruop_Index,CompanyNo,JobGruopNo,JobGroupName,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] JobGruopIndex jobGruopIndex)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            jobGruopIndex.CreateBy = Convert.ToInt32(User.Identity.Name);
            jobGruopIndex.CreateBy_Time = DateTime.Now;
            jobGruopIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
            jobGruopIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                jobGruopIndex.JobGruopNo = AC.GetAutoCodeJobGroup(CompanyNo);
                jobGruopIndex.CompanyNo = CompanyNo;
                jobGruopIndex.Status = 1;
                db.JobGruopIndex.Add(jobGruopIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobGruopIndex);
        }

        // GET: JobGroup/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobGruopIndex jobGruopIndex = db.JobGruopIndex.Find(id);
            if (jobGruopIndex == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (jobGruopIndex.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(jobGruopIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(jobGruopIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(jobGruopIndex.Status);
            return View(jobGruopIndex);
        }

        // POST: JobGroup/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobGruop_Index,CompanyNo,JobGruopNo,JobGroupName,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] JobGruopIndex jobGruopIndex)
        {
            var hrInfo = db.HRInfo.Where(x => x.JobGruop_Index == jobGruopIndex.JobGruop_Index);
            if (jobGruopIndex.Status == 2)
            {
                if (hrInfo.Count() > 0)
                {
                    foreach (var item in hrInfo)
                    {
                        if (item.OnJobStatus != 4)
                        {
                            ModelState.AddModelError("Status", "請先將此群組人員調離或更改為離職狀態");
                            break;
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                jobGruopIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                jobGruopIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(jobGruopIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(jobGruopIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(jobGruopIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(jobGruopIndex.Status);
            return View(jobGruopIndex);
        }

        // GET: JobGroup/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobGruopIndex jobGruopIndex = db.JobGruopIndex.Find(id);
        //    if (jobGruopIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobGruopIndex);
        //}

        //// POST: JobGroup/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    JobGruopIndex jobGruopIndex = db.JobGruopIndex.Find(id);
        //    db.JobGruopIndex.Remove(jobGruopIndex);
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
