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
    public class HRInfoesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        JobStatus JS = new JobStatus();
        // GET: HRInfoes
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            string CompanyNo = Session["ComnpanyNo"].ToString();
            var hRInfo = db.HRInfo.OrderBy(x => x.EmpNo).Where(x => x.CompanyNo == CompanyNo);
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            if (Select != null)
            {
                if (Select == 1)
                {
                    hRInfo = hRInfo.Where(x => x.EmpNo.StartsWith(Search));
                }
                else if(Select ==2)
                {
                    hRInfo = hRInfo.Where(x => x.EmpName.Contains(Search) || x.EmpName.Contains(Search));
                }
                else if (Select == 3)
                {
                    int depindex = Convert.ToInt32(Search);
                    ViewBag.depIndex = depindex;
                    hRInfo = hRInfo.Where(x => x.Dep_Index == depindex);
                }
                else if (Select == 4)
                {
                    int groupindex = Convert.ToInt32(Search);
                    ViewBag.groupIndex = groupindex; 
                    hRInfo = hRInfo.Where(x => x.JobGruop_Index == groupindex);
                }
                else
                {
                    int Onjobstatus = int.Parse(Search);
                    ViewBag.Onjobstatus = Onjobstatus;
                    hRInfo = hRInfo.Where(x => x.OnJobStatus == Onjobstatus);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    hRInfo = hRInfo.Where(x => x.EmpNo.StartsWith(Search) || x.EmpName.Contains(Search) || x.EmpName.Contains(Search)
                        || x.PrivatePhone.Contains(Search) || x.ComPhone.Contains(Search) || x.EmpID.StartsWith(Search));
                }
            }
            var joinHr = hRInfo.GroupJoin(hRInfo, x => x.EmpIndex, p => p.JobManager, (x, p) => new { x.EmpIndex, x.EmpName, JobManager = p.Select(k => k.EmpName).FirstOrDefault().ToString(), 
                x.JobTitle, x.ComPhone, x.ComExt,x.EmpNo,x.OnJobStatus });
            List<HrInfoViewModel> JoinedHr = new List<HrInfoViewModel>();
            foreach (var item in joinHr)
            {
                var hrinfo = new HrInfoViewModel();
                hrinfo.EmpIndex = item.EmpIndex;
                hrinfo.EmpName = item.EmpName;
                hrinfo.EmpNo = item.EmpNo;
                hrinfo.JobTitle = item.JobTitle;
                hrinfo.JobManager = item.JobManager;
                hrinfo.ComPhone = item.ComPhone;
                hrinfo.ComExt = item.ComExt;
                hrinfo.OnJobStatus = item.OnJobStatus;
                JoinedHr.Add(hrinfo);
            }
            JoinedHr.OrderBy(x => x.EmpNo);
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            CreateList searchList = new CreateList();
            searchList.addItem(1, "員工代號");
            searchList.addItem(2, "姓名");
            searchList.addItem(3, "部門");
            searchList.addItem(4, "群組");
            searchList.addItem(5, "在職狀態");
            searchList.setList(null);
            ViewBag.DepIndex = new SelectList(db.DepIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 1), "Dep_Index", "DepName");
            ViewBag.JobGroup = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == CompanyNo && x.Status == 1), "JobGruop_Index", "JobGroupName");
            var PermiGroups = db.PermiGruop.Where(x => x.CompanyNo == CompanyNo);
            ViewBag.PermiGroups = new MultiSelectList(PermiGroups, "PermiGpIndex", "PermiGpName");
            ViewBag.SelectBar = searchList.CreatedList;
            ViewData["DataCount"] = joinHr.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = JoinedHr.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }
        public ActionResult GetStatusList(int? id)
        {
            CreateList searchOption = new CreateList();
            searchOption.addItem(1, "在職");
            searchOption.addItem(2, "留職停薪");
            searchOption.addItem(3, "育嬰假");
            searchOption.addItem(4, "離職");
            searchOption.setList(id);
            return PartialView("_SearchPartial", searchOption.CreatedList);
        }
        public ActionResult GetDep(int? id)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            var dep = new SelectList(db.DepIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "Dep_Index", "DepName");
            if (id != null)
            {
                dep = new SelectList(db.DepIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "Dep_Index", "DepName",id);
            }           
            return PartialView("_SearchPartial", dep);
        }
        public ActionResult GetGroup(int? id)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            var JobGroup = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "JobGruop_Index", "JobGroupName");
            if (id != null)
            {
                JobGroup = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "JobGruop_Index", "JobGroupName", id);
            }
            return PartialView("_SearchPartial", JobGroup);
        }
        // GET: HRInfoes/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HRInfo hRInfo = db.HRInfo.Find(id);
        //    if (hRInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hRInfo);
        //}

        // GET: HRInfoes/Create
        public ActionResult Create()
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            ViewBag.OnJobStatus = new SelectList(JS.status,"key","value");
            ViewBag.JobManager = new SelectList(db.HRInfo.Where(x => x.CompanyNo == compyNo && x.OnJobStatus != 4 ), "EmpIndex", "EmpName");
            ViewBag.Dep_Index = new SelectList(db.DepIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "Dep_Index", "DepName");
            ViewBag.JobGruop_Index = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "JobGruop_Index", "JobGroupName");
            ViewBag.JobLevel_Index = new SelectList(db.JobLevelIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "JobLevel_Index", "JobLevel");
            return View();
        }

        // POST: HRInfoes/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmpIndex,EmpNo,CompanyNo,Dep_Index,JobLevel_Index,JobGruop_Index,EmpEnName,EmpName,JobManager,JobTitle,EmpID,EmpBday,ComPhone,ComExt,ComEmail,HomeAddres,ContactAddres,PrivateMail,PrivatePhone,UrgentName,RelationUrgent,UrgentPhone,OnJobStatus,OnBoardDate,ResignDate,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,AllowLogIn")] HRInfo hRInfo, int nation)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            int Person = Convert.ToInt32(User.Identity.Name);
            AutoCode ac = new AutoCode();
            hRInfo.EmpNo = ac.GetAutoCodeHR(compyNo);
            IDValidator IDvalid = new IDValidator();
            if (nation == 1)
            {
                if (hRInfo.EmpID != null)
                {
                    if (!IDValidator.IDChk(hRInfo.EmpID))
                    {
                        ModelState.AddModelError("EmpID", "身分證字號不正確");
                    }
                }  
            }
           
            var id = db.HRInfo.Where(x => x.EmpID == hRInfo.EmpID && x.CompanyNo == compyNo);
            if (id.Count()>0)
            {
                ModelState.AddModelError("EmpID", "身份證字號重複");
            }
            
            if (ModelState.IsValid)
            {
                hRInfo.CreateBy = Person;
                hRInfo.CreateBy_Time = DateTime.Now;
                hRInfo.UpdateBy = Person;
                hRInfo.UpdateBy_Time = DateTime.Now;
                hRInfo.CompanyNo = compyNo;
                if (hRInfo.OnJobStatus == 4)
                {
                    hRInfo.AllowLogIn = false;
                }
                else
                {
                    hRInfo.AllowLogIn = true;
                }
                PasMng Pass = new PasMng();
                db.HRInfo.Add(hRInfo);
                db.SaveChanges();
                Pass.EmpIndex = hRInfo.EmpIndex;
                Pass.oldpsd = hRInfo.EmpNo+"0";
                Pass.newpsd = hRInfo.EmpNo+"0";
                Pass.CompanyNo = compyNo;
                Pass.CreateBy = Person;
                Pass.CreateBy_Time = DateTime.Now;
                Pass.UpdateBy_Time = DateTime.Now;
                Pass.UpdateBy = Person;
                db.PasMng.Add(Pass);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            hRInfo.CompanyNo = compyNo;
            getViewData(hRInfo);
           
            return View(hRInfo);
        }
        public void getViewData(HRInfo hRInfo)
        {
            ViewBag.OnJobStatus = new SelectList(JS.status, "key", "value", hRInfo.OnJobStatus);
            if (hRInfo.JobManager == null)
            {
                ViewBag.JobManager = new SelectList(db.HRInfo.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.OnJobStatus != 4), "EmpIndex", "EmpName");
            }
            else
            {
                ViewBag.JobManager = new SelectList(db.HRInfo.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.OnJobStatus != 4), "EmpIndex", "EmpName",hRInfo.JobManager);
            }
            ViewBag.JobManager = new SelectList(db.HRInfo.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.OnJobStatus != 4), "EmpIndex", "EmpName",hRInfo.JobManager);
            if (hRInfo.OnJobStatus != 4)
            {
                ViewBag.Dep_Index = new SelectList(db.DepIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.Status == 1), "Dep_Index", "DepName", hRInfo.Dep_Index);
                ViewBag.JobGruop_Index = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.Status == 1), "JobGruop_Index", "JobGroupName", hRInfo.JobGruop_Index);
                ViewBag.JobLevel_Index = new SelectList(db.JobLevelIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo && x.Status == 1), "JobLevel_Index", "JobLevel", hRInfo.JobLevel_Index);
            }
            else
            {
                
                ViewBag.Dep_Index = new SelectList(db.DepIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo), "Dep_Index", "DepName", hRInfo.Dep_Index);
                ViewBag.JobGruop_Index = new SelectList(db.JobGruopIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo), "JobGruop_Index", "JobGroupName", hRInfo.JobGruop_Index);
                ViewBag.JobLevel_Index = new SelectList(db.JobLevelIndex.Where(x => x.CompanyNo == hRInfo.CompanyNo), "JobLevel_Index", "JobLevel", hRInfo.JobLevel_Index);
            }
           
        }

        // GET: HRInfoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HRInfo hRInfo = db.HRInfo.Find(id);
            if (hRInfo == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (hRInfo.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            getViewData(hRInfo);
            ViewBag.CreateBy = db.HRInfo.Find(hRInfo.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hRInfo.UpdateBy).EmpName;
            return View(hRInfo);
        }

        // POST: HRInfoes/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmpIndex,EmpNo,CompanyNo,Dep_Index,JobLevel_Index,JobGruop_Index,EmpEnName,EmpName,JobManager,JobTitle,EmpID,EmpBday,ComPhone,ComExt,ComEmail,HomeAddres,ContactAddres,PrivateMail,PrivatePhone,UrgentName,RelationUrgent,UrgentPhone,OnJobStatus,OnBoardDate,ResignDate,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,AllowLogIn")] HRInfo hRInfo, int nation)
        {
            if (nation == 1)
            {
                if (hRInfo.EmpID != null)
                {
                    if (!IDValidator.IDChk(hRInfo.EmpID))
                    {
                        ModelState.AddModelError("EmpID", "身分證字號不正確");
                    }
                }
            }
            if (ModelState.IsValid)
            {
                if (hRInfo.AllowLogIn && hRInfo.OnJobStatus == 4)
                {
                    hRInfo.AllowLogIn = false;
                }
                else if (hRInfo.AllowLogIn)
                {
                    var Login = db.logInLog.Where(x => x.EmpIndex == hRInfo.EmpIndex);
                    foreach (var item in Login)
                    {
                        item.ErrorLog = false;
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
                hRInfo.UpdateBy_Time = DateTime.Now;
                hRInfo.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(hRInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(hRInfo.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hRInfo.UpdateBy).EmpName;
            getViewData(hRInfo);
            return View(hRInfo);
        }
        [HttpPost]
        public ActionResult ChangeDepAndGroup(HRinfoDepGroupViewModel Change)
        {
            if (Change.EmpIndex == null)
            {
                return Content("請選擇要調動的員工");
            }
            if (Change.DepIndex == null && Change.JobGroup == null)
            {
                return Content("請選擇部門或群組");
            }
            if (Change.DepIndex != null)
            {
                foreach (int i in Change.EmpIndex)
                {
                   HRInfo hrInfo = db.HRInfo.Find(i);
                   hrInfo.Dep_Index = Convert.ToInt32(Change.DepIndex);
                   hrInfo.UpdateBy = Convert.ToInt32(User.Identity.Name);
                   hrInfo.UpdateBy_Time = DateTime.Now; 
                   db.Entry(hrInfo).State = EntityState.Modified;
                }
            }
            if (Change.JobGroup != null)
            {
                foreach (int i in Change.EmpIndex)
                {
                    var hrInfo = db.HRInfo.Find(i);
                    hrInfo.UpdateBy = Convert.ToInt32(User.Identity.Name);
                    hrInfo.UpdateBy_Time = DateTime.Now;
                    hrInfo.JobGruop_Index = Convert.ToInt32(Change.JobGroup);
                    db.Entry(hrInfo).State = EntityState.Modified;
                }
            }
            try
            {
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                return Content(ex.ToString());
            }
            
            return Content("部門與群組更改完成");
        }
      
        [HttpPost]
        public ActionResult ResetPass(int? id)
        {
            if (id == null)
            {
                return Content("重置失敗");
            }
            var hrInfo = db.HRInfo.Find(id);
            var passWord = db.PasMng.Where(x => x.EmpIndex == id).FirstOrDefault();
            passWord.oldpsd = hrInfo.EmpNo + "0";
            passWord.newpsd = hrInfo.EmpNo + "0";
            passWord.UpdateBy = Convert.ToInt32(User.Identity.Name);
            passWord.UpdateBy_Time = DateTime.Now;
            db.Entry(passWord).State = EntityState.Modified;
            db.SaveChanges();
            return Content("已將密碼重置為員工編號+尾碼0");
        }
        // GET: HRInfoes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    HRInfo hRInfo = db.HRInfo.Find(id);
        //    if (hRInfo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(hRInfo);
        //}

        // POST: HRInfoes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    HRInfo hRInfo = db.HRInfo.Find(id);
        //    db.HRInfo.Remove(hRInfo);
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
