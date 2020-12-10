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
    [Authorize]
    [AuthorizeCompny]
    public class UpDivisionsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;

        // GET: UpDivisions
        public ActionResult Index(int? Status, string Cname, string PDivision_Code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            var updivision = db.UpDivision.OrderBy(x => x.PDivision_Code).Where(x =>( x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(PDivision_Code))//找尋國家
            {
                ViewBag.PDivision_Code = PDivision_Code;
                updivision = updivision.Where(x => x.PDivision_Code.Contains(PDivision_Code));
            }
            if (!string.IsNullOrEmpty(Cname))//找尋國家中文名稱
            {
                ViewBag.Cname = Cname;
                updivision = updivision.Where(x => x.Cname.Contains(Cname));
            }
            if (Status != null)//找尋是否完成
            {
                ViewBag.Status = Status;
                updivision = updivision.Where(x => x.Status == Status);
            }
            ViewData["DataCount"] = updivision.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = updivision.ToPagedList(CurrentPage, pagesize);
            ViewData["StatusNum"] = GetStuatus.DefualStatus();
            return View(result);
        }

        // GET: UpDivisions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpDivision upDivision = db.UpDivision.Find(id);
            if (upDivision == null)
            {
                return HttpNotFound();
            }
            return View(upDivision);
        }

        // GET: UpDivisions/Create
        public ActionResult Create()
        {
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            return View();
        }

        // POST: UpDivisions/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PDivisionIndex,PDivision_Code,Cname,Status")] UpDivision upDivision)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int dvisionCount = db.UpDivision.Where(x => x.PDivision_Code == upDivision.PDivision_Code && x.CompanyNo == Company).Count();
            if (dvisionCount > 0)
            {
                ModelState.AddModelError("PDivision_Code", "線別代碼重複");
            }
            if (ModelState.IsValid)
            {
                upDivision.UpdateBy = Convert.ToInt32(User.Identity.Name);
                upDivision.CreateBy = Convert.ToInt32(User.Identity.Name);
                upDivision.CompanyNo = Company;
                upDivision.CreateBy_Time = DateTime.Now;
                upDivision.UpdateBy_Time = DateTime.Now;
                db.UpDivision.Add(upDivision);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Status"] = GetStuatus.GetStatus(upDivision.Status); //取得檔案狀態DropDownList 
            return View(upDivision);
        }

        // GET: UpDivisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpDivision upDivision = db.UpDivision.Find(id);
            if (upDivision == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (upDivision.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(upDivision.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(upDivision.UpdateBy).EmpName;
            ViewData["Status"] = GetStuatus.GetStatus(upDivision.Status); //取得檔案狀態DropDownList 
            return View(upDivision);
        }

        // POST: UpDivisions/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,PDivisionIndex,PDivision_Code,Cname,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] UpDivision upDivision)
        {
            if (ModelState.IsValid)
            {
                upDivision.UpdateBy_Time = DateTime.Now;
                upDivision.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(upDivision).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(upDivision.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(upDivision.UpdateBy).EmpName;
            ViewData["Status"] = GetStuatus.GetStatus(upDivision.Status); //取得檔案狀態DropDownList 
            return View(upDivision);
        }

        // GET: UpDivisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UpDivision upDivision = db.UpDivision.Find(id);
            if (upDivision == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (upDivision.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            if (upDivision.Division.Where(x => x.Status == 1 || x.Status == 2).Count() > 0)
            {
                ViewBag.ShowDetail = "請先刪除小線別";
            }
            return View(upDivision);
        }

        // POST: UpDivisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UpDivision upDivision = db.UpDivision.Find(id);
  
            upDivision.Status = 3;
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
