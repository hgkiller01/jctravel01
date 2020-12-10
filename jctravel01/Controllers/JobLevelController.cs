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
    public class JobLevelController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: JobLevel
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            var JobLevel = db.JobLevelIndex.OrderBy(x => x.JobLevelCode).Where(x => x.CompanyNo == CompanyNo);
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            if (Select != null)
            {
                if (Select == 1)
                {
                    JobLevel = JobLevel.Where(x => x.JobLevelCode.StartsWith(Search));
                }
                else
                {
                    JobLevel = JobLevel.Where(x => x.JobLevel.Contains(Search));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    JobLevel = JobLevel.Where(x => x.JobLevelCode.StartsWith(Search) || x.JobLevel.Contains(Search));
                }
            }
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            Dictionary<int, string> searchList = new Dictionary<int, string>();
            searchList.Add(1, "職等代號");
            searchList.Add(2, "職等名稱");
            ViewBag.SelectBar = new SelectList(searchList, "key", "value");
            ViewData["DataCount"] = JobLevel.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = JobLevel.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: JobLevel/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobLevelIndex jobLevelIndex = db.JobLevelIndex.Find(id);
        //    if (jobLevelIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobLevelIndex);
        //}

        // GET: JobLevel/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobLevel/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobLevel_Index,CompanyNo,JobLevelCode,JobLevel,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] JobLevelIndex jobLevelIndex)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            jobLevelIndex.CreateBy = Convert.ToInt32(User.Identity.Name);
            jobLevelIndex.CreateBy_Time = DateTime.Now;
            jobLevelIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
            jobLevelIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                jobLevelIndex.JobLevelCode = AC.GetAutoCodeJobLevel(CompanyNo);
                jobLevelIndex.CompanyNo = CompanyNo;
                jobLevelIndex.Status = 1;
                db.JobLevelIndex.Add(jobLevelIndex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobLevelIndex);
        }

        // GET: JobLevel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobLevelIndex jobLevelIndex = db.JobLevelIndex.Find(id);
            if (jobLevelIndex == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (jobLevelIndex.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(jobLevelIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(jobLevelIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(jobLevelIndex.Status);
            return View(jobLevelIndex);
        }

        // POST: JobLevel/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobLevel_Index,CompanyNo,JobLevelCode,JobLevel,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Status")] JobLevelIndex jobLevelIndex)
        {
            var hrInfo = db.HRInfo.Where(x => x.JobLevel_Index == jobLevelIndex.JobLevel_Index);
            if (jobLevelIndex.Status == 2)
            {
                if (hrInfo.Count() > 0)
                {
                    foreach (var item in hrInfo)
                    {
                        if (item.OnJobStatus != 4)
                        {
                            ModelState.AddModelError("Status", "請先將此職等人員調離或更改為離職狀態");
                            break;

                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                jobLevelIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                jobLevelIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(jobLevelIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(jobLevelIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(jobLevelIndex.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(jobLevelIndex.Status);
            return View(jobLevelIndex);
        }

        // GET: JobLevel/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    JobLevelIndex jobLevelIndex = db.JobLevelIndex.Find(id);
        //    if (jobLevelIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(jobLevelIndex);
        //}

        // POST: JobLevel/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    JobLevelIndex jobLevelIndex = db.JobLevelIndex.Find(id);
        //    db.JobLevelIndex.Remove(jobLevelIndex);
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
