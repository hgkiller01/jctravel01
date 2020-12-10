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
using System.Text.RegularExpressions;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeCompny]
    public class DivisionsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: Divisions
        public ActionResult Index(int? Status, string Cname, string Division_code, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            var division = db.Division.OrderBy(x => x.Division_code).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Division_code))//找尋國家
            {
                ViewBag.Division_code = Division_code;
                division = division.Where(x => x.Division_code.Contains(Division_code));
            }
            if (!string.IsNullOrEmpty(Cname))//找尋國家中文名稱
            {
                ViewBag.Cname = Cname;
                division = division.Where(x => x.Cname.Contains(Cname));
            }
            if (Status != null)//找尋是否完成
            {
                ViewBag.Status = Status;
                division = division.Where(x => x.Status == Status);
            }
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            ViewData["DataCount"] = division.Count();
            ViewData["StatusNum"] = GetStuatus.DefualStatus();
            var reslut = division.ToPagedList(CurrentPage,pagesize);
            return View(reslut);
        }

        // GET: Divisions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Division.Find(id);
            if (division == null)
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // GET: Divisions/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var updivision = db.UpDivision.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new {x.PDivisionIndex,Cname = x.PDivision_Code+" "+x.Cname });
            ViewBag.PDivisionIndex = new SelectList(updivision, "PDivisionIndex", "Cname");
            ViewData["Status"] = GetStuatus.GetStatus(); //取得檔案狀態DropDownList
            return View();
        }

        // POST: Divisions/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DivisionIndex,PDivisionIndex,Division_code,Cname,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] Division division)
        {
            string Company = Session["ComnpanyNo"].ToString();
            division.Division_code = db.UpDivision.Find(division.PDivisionIndex).PDivision_Code + division.Division_code;
            Regex regex = new Regex(@"[0-9]{1,1}[a-zA-Z]{1,1}$",RegexOptions.IgnoreCase);
            if (!regex.IsMatch(division.Division_code))
            {
                ModelState.AddModelError("Division_code", "代碼錯誤");
            }
            
            int divisionCount = db.Division.Where(x => x.Division_code == division.Division_code && x.CompanyNo == Company).Count();
            if (divisionCount > 0)
            {
                ModelState.AddModelError("Division_code", "小線別代碼重複");
            }
            division.CreateBy_Time = DateTime.Now;
            division.UpdateBy_Time = DateTime.Now;
            division.CreateBy = Convert.ToInt32(User.Identity.Name);
            division.UpdateBy = Convert.ToInt32(User.Identity.Name);
            if (ModelState.IsValid)
            {
                ModelState.Clear();
                division.CompanyNo = Company;
                db.Division.Add(division);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["Status"] = GetStuatus.GetStatus(division.Status); //取得檔案狀態DropDownList
            var updivision = db.UpDivision.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.PDivisionIndex, Cname = x.PDivision_Code + " " + x.Cname });
            ViewBag.PDivisionIndex = new SelectList(updivision, "PDivisionIndex", "Cname", division.PDivisionIndex);
            return View(division);
        }

        // GET: Divisions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Division.Find(id);
            if (division == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (division.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(division.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(division.UpdateBy).EmpName;
            ViewData["Status"] = GetStuatus.GetStatus(division.Status); //取得檔案狀態DropDownList
            ViewBag.PDivisionIndex = new SelectList(db.UpDivision, "PDivisionIndex", "Cname", division.PDivisionIndex);
            return View(division);
        }

        // POST: Divisions/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DivisionIndex,PDivisionIndex,Division_code,Cname,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,CompanyNo")] Division division)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (ModelState.IsValid)
            {
                division.UpdateBy = Convert.ToInt32(User.Identity.Name);
                division.UpdateBy_Time = DateTime.Now;
                db.Entry(division).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(division.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(division.UpdateBy).EmpName;
            ViewData["Status"] = GetStuatus.GetStatus(division.Status); //取得檔案狀態DropDownList
            var updivision = db.UpDivision.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.PDivisionIndex, Cname = x.PDivision_Code + " " + x.Cname });
            ViewBag.PDivisionIndex = new SelectList(updivision, "PDivisionIndex", "Cname", division.PDivisionIndex);
            return View(division);
        }

        // GET: Divisions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Division division = db.Division.Find(id);
            if (division == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (division.CompanyNo != Company && division.CompanyNo != "00001")
            {
                return HttpNotFound();
            }
            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Division division = db.Division.Find(id);
            division.Status = 3;
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
