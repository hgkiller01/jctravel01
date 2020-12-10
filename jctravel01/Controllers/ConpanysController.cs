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
    [AuthorizeOurCompny]
    public class ConpanysController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: Conpanys
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            IEnumerable<CoIndex> compny = db.CoIndex.ToList();
            if (Select != null)
            {
                if (Select == 1)
                {
                 compny = compny.Where(x => x.CompanyNo.Contains(Search));
                
                }
                else if(Select == 2)
                {
                     compny = compny.Where(x => x.ComName.Contains(Search));
                    
                }
                else
                {
                    compny = compny.Where(x => x.TaxID.Contains(Search));
                    
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    compny = compny.Where(x => x.CompanyNo.Contains(Search) || x.ComName.Contains(Search) || x.TaxID.Contains(Search));
                }
            }
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            Dictionary<int, string> searchList = new Dictionary<int, string>();
            searchList.Add(1, "公司代號");
            searchList.Add(2, "公司名稱");
            searchList.Add(3, "統一編號");
            ViewBag.SelectBar = new SelectList(searchList, "key", "value");
            ViewData["DataCount"] = compny.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = compny.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Conpanys/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CoIndex coIndex = db.CoIndex.Find(id);
        //    if (coIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coIndex);
        //}

        // GET: Conpanys/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conpanys/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CompanyNo,Status,ComName,TaxID,ContactPhone,ContactName,Co_addres,CoIP,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] CoIndex coIndex)
        {
            AutoCode AC = new AutoCode();
            int Persion = Convert.ToInt32(User.Identity.Name);
            coIndex.CreateBy = Persion;
            coIndex.UpdateBy = Persion;
            coIndex.CreateBy_Time = DateTime.Now;
            coIndex.UpdateBy_Time = DateTime.Now;
            if (ModelState.IsValid)
            {
                coIndex.CompanyNo = AC.GetAutoCodeCompy();
   
                coIndex.Status = true;
                db.CoIndex.Add(coIndex);
                db.SaveChanges();
                #region 部門
                DepIndex dep = new DepIndex();
                dep.DepNo = AC.GetAutoCodeDep(coIndex.CompanyNo);
                dep.CompanyNo = coIndex.CompanyNo;
                dep.DepName = "產品";
                dep.Status = 1;
                dep.CreateBy = Persion;
                dep.CreateBy_Time = DateTime.Now;
                dep.UpdateBy = Persion;
                dep.UpdateBy_Time = DateTime.Now;
                db.DepIndex.Add(dep);
                DepIndex dep2 = dep.Clone();
                dep2.DepName = "業務";
                dep2.DepNo = (int.Parse(dep.DepNo) + 1).ToString("00");
                db.DepIndex.Add(dep2);
                DepIndex dep3 = dep.Clone();
                dep3.DepName = "導領";
                dep3.DepNo = (int.Parse(dep2.DepNo) + 1).ToString("00");
                db.DepIndex.Add(dep3);
                DepIndex dep4 = dep.Clone();
                dep4.DepNo = (int.Parse(dep3.DepNo) + 1).ToString("00");
                dep4.DepName = "財務";
                db.DepIndex.Add(dep4);
                DepIndex dep5 = dep.Clone();
                dep5.DepNo = (int.Parse(dep4.DepNo) + 1).ToString("00");
                dep5.DepName = "管理";
                db.DepIndex.Add(dep5);
                db.SaveChanges();
                #endregion
                #region 群組
                JobGruopIndex JobG = new JobGruopIndex();
                JobG.JobGroupName = "預設組";
                JobG.JobGruopNo = AC.GetAutoCodeJobGroup(coIndex.CompanyNo);
                JobG.CompanyNo = coIndex.CompanyNo;
                JobG.Status = 1;
                JobG.CreateBy = Persion;
                JobG.CreateBy_Time = DateTime.Now;
                JobG.UpdateBy = Persion;
                JobG.UpdateBy_Time = DateTime.Now;
                db.JobGruopIndex.Add(JobG);
                #endregion
                #region 職等
                JobLevelIndex JobL = new JobLevelIndex();
                JobL.JobLevel = "工程師";
                JobL.JobLevelCode = AC.GetAutoCodeJobLevel(coIndex.CompanyNo);
                JobL.CompanyNo = coIndex.CompanyNo;
                JobL.Status = 1;
                JobL.CreateBy = Persion;
                JobL.CreateBy_Time = DateTime.Now;
                JobL.UpdateBy = Persion;
                JobL.UpdateBy_Time = DateTime.Now;
                db.JobLevelIndex.Add(JobL);
                JobLevelIndex JobL2 = JobL.Clone();
                JobL2.JobLevelCode = (int.Parse(JobL.JobLevelCode) + 1).ToString("00");
                JobL2.JobLevel = "業務";
                db.JobLevelIndex.Add(JobL2);
                JobLevelIndex JobL3 = JobL.Clone();
                JobL3.JobLevelCode = (int.Parse(JobL2.JobLevelCode) + 1).ToString("00");
                JobL3.JobLevel = "專員";
                db.JobLevelIndex.Add(JobL3);
                JobLevelIndex JobL4 = JobL.Clone();
                JobL4.JobLevelCode = (int.Parse(JobL3.JobLevelCode) + 1).ToString("00");
                JobL4.JobLevel = "協理";
                db.JobLevelIndex.Add(JobL4);
                JobLevelIndex JobL5 = JobL.Clone();
                JobL5.JobLevelCode = (int.Parse(JobL4.JobLevelCode) + 1).ToString("00");
                JobL5.JobLevel = "經理";
                db.JobLevelIndex.Add(JobL5);
                JobLevelIndex JobL6 = JobL.Clone();
                JobL6.JobLevelCode = (int.Parse(JobL5.JobLevelCode) + 1).ToString("00");
                JobL6.JobLevel = "總經理";
                db.JobLevelIndex.Add(JobL6);
                JobLevelIndex JobL7 = JobL.Clone();
                JobL7.JobLevelCode = (int.Parse(JobL6.JobLevelCode) + 1).ToString("00");
                JobL7.JobLevel = "主任";
                db.JobLevelIndex.Add(JobL7);
                JobLevelIndex JobL8 = JobL.Clone();
                JobL8.JobLevelCode = (int.Parse(JobL7.JobLevelCode) + 1).ToString("00");
                JobL8.JobLevel = "副主任";
                db.JobLevelIndex.Add(JobL8);
                JobLevelIndex JobL9 = JobL.Clone();
                JobL9.JobLevelCode = (int.Parse(JobL8.JobLevelCode) + 1).ToString("00");
                JobL9.JobLevel = "副理";
                db.JobLevelIndex.Add(JobL9);

                #endregion                              
                
                db.SaveChanges();
                #region 人員
                HRInfo Hr = new HRInfo();
                Hr.CompanyNo = coIndex.CompanyNo;
                Hr.EmpNo = AC.GetAutoCodeHR(Hr.CompanyNo);
                Hr.AllowLogIn = true;
                Hr.ComEmail = "abc@yahooo.com";
                Hr.ComPhone = coIndex.ContactPhone;
                Hr.ContactAddres = coIndex.Co_addres;
                Hr.EmpBday = DateTime.Now.Date;
                Hr.EmpID = "A123456789";
                Hr.EmpName = "管理人員";
                Hr.JobTitle = "專人";
                Hr.OnBoardDate = DateTime.Now.Date;
                Hr.HomeAddres = coIndex.Co_addres;
                Hr.OnJobStatus = 1;
                Hr.PrivatePhone = "000000000";
                Hr.UrgentName = "無";
                Hr.UrgentPhone = "000000000";
                Hr.Dep_Index = dep5.Dep_Index;
                Hr.JobGruop_Index = JobG.JobGruop_Index;
                Hr.JobLevel_Index = JobL.JobLevel_Index;
                Hr.RelationUrgent = "無";
                Hr.CreateBy = Persion;
                Hr.CreateBy_Time = DateTime.Now;
                Hr.UpdateBy = Persion;
                Hr.UpdateBy_Time = DateTime.Now;
                #endregion
                db.HRInfo.Add(Hr);
                db.SaveChanges();
                #region 密碼
                PasMng Pass = new PasMng();
                Pass.CompanyNo = coIndex.CompanyNo;
                Pass.EmpIndex = Hr.EmpIndex;
                Pass.newpsd = Hr.EmpNo + "0";
                Pass.oldpsd = Hr.EmpNo + "0";
                Pass.UpdateBy = Persion;
                Pass.UpdateBy_Time = DateTime.Now;
                Pass.CreateBy = Persion;
                Pass.CreateBy_Time = DateTime.Now;
                #endregion
                db.PasMng.Add(Pass);
                db.SaveChanges();
                //TODO 要插入資料
                return RedirectToAction("Index");
            }

            return View(coIndex);
        }

        // GET: Conpanys/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoIndex coIndex = db.CoIndex.Find(id);
            if (coIndex == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreateBy = db.HRInfo.Find(coIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(coIndex.UpdateBy).EmpName;
            return View(coIndex);
        }

        // POST: Conpanys/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CompanyNo,Status,ComName,TaxID,ContactPhone,ContactName,Co_addres,CoIP,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] CoIndex coIndex)
        {
            if (ModelState.IsValid)
            {
                if (!coIndex.Status)
                {
                    foreach (var item in coIndex.HRInfo)
                    {
                        item.AllowLogIn = false;
                    }
                }
                else
                {
                    foreach (var item in coIndex.HRInfo)
                    {
                        if (!item.AllowLogIn && item.OnJobStatus != 4)
                        {
                            item.AllowLogIn = true;
                        }
                    }
                }
                coIndex.UpdateBy = Convert.ToInt32(User.Identity.Name);
                coIndex.UpdateBy_Time = DateTime.Now;
                db.Entry(coIndex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CreateBy = db.HRInfo.Find(coIndex.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(coIndex.UpdateBy).EmpName;
            return View(coIndex);
        }

        // GET: Conpanys/Delete/5
        //public ActionResult Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    CoIndex coIndex = db.CoIndex.Find(id);
        //    if (coIndex == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(coIndex);
        //}

        // POST: Conpanys/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(string id)
        //{
        //    CoIndex coIndex = db.CoIndex.Find(id);
        //    db.CoIndex.Remove(coIndex);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
