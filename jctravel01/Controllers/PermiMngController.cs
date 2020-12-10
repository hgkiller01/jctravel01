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
    public class PermiMngController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: PermiMng
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
                else if (Select == 2)
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
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            CreateList searchList = new CreateList();
            searchList.addItem(1, "員工代號");
            searchList.addItem(2, "姓名");
            searchList.addItem(3, "部門");
            searchList.addItem(4, "群組");
            searchList.addItem(5, "在職狀態");
            searchList.setList(null);
            var PermiGroups = db.PermiGruop.Where(x => x.CompanyNo == CompanyNo);
            ViewBag.PermiGroups = new MultiSelectList(PermiGroups, "PermiGpIndex", "PermiGpName");
            ViewBag.SelectBar = searchList.CreatedList;
            ViewData["DataCount"] = hRInfo.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = hRInfo.ToPagedList(CurrentPage, pagesize);
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
                dep = new SelectList(db.DepIndex.Where(x => x.CompanyNo == compyNo && x.Status == 1), "Dep_Index", "DepName", id);
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

        // GET: PermiMng/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PermiMng permiMng = db.PermiMng.Find(id);
        //    if (permiMng == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(permiMng);
        //}

        // GET: PermiMng/Create
        //public ActionResult Create()
        //{
        //    ViewBag.CompanyNo = new SelectList(db.CoIndex, "CompanyNo", "ComName");
        //    ViewBag.EmpIndex = new SelectList(db.HRInfo, "EmpIndex", "EmpNo");
        //    ViewBag.PermiGpIndex = new SelectList(db.PermiGruop, "PermiGpIndex", "PermiGpNo");
        //    return View();
        //}

        // POST: PermiMng/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PMngIndex,CompanyNo,EmpIndex,PermiGpIndex,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time,Main")] PermiMng permiMng)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.PermiMng.Add(permiMng);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.CompanyNo = new SelectList(db.CoIndex, "CompanyNo", "ComName", permiMng.CompanyNo);
        //    ViewBag.EmpIndex = new SelectList(db.HRInfo, "EmpIndex", "EmpNo", permiMng.EmpIndex);
        //    ViewBag.PermiGpIndex = new SelectList(db.PermiGruop, "PermiGpIndex", "PermiGpNo", permiMng.PermiGpIndex);
        //    return View(permiMng);
        //}

        // GET: PermiMng/Edit/5
        public ActionResult Edit(int? id)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            List<PermiGruop> groupPermi = db.PermiGruop.Where(x => x.CompanyNo == compyNo).ToList();
            var permiMng = db.PermiMng.Where(x => x.EmpIndex == id);
            if (permiMng == null)
            {
                return HttpNotFound();
            }
            List<AllGroup> all = new List<AllGroup>();
            foreach (var item in groupPermi)
            {
                all.Add(new AllGroup()
                {
                    PermiGpIndex = item.PermiGpIndex,
                    PermiGpName = item.PermiGpName,
                    IsSelected = false
                });
            }
            foreach (var item in permiMng)
            {
                if (item.Main)
                {
                    foreach (var item2 in all)
                    {
                        if (item.PermiGpIndex == item2.PermiGpIndex)
                        {
                            item2.IsSelected = true;
                        }
                    }
                }
            }
            PermiList Pl = new PermiList();
            Pl.EmpIndex = Convert.ToInt32(id);
            Pl.PermiGroupList = all;
            return PartialView("_PermiEdit", Pl);
        }

        // POST: PermiMng/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int[] GroupIndex, int EmpIndex)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            if (ModelState.IsValid)
            {
                var compyPermi = db.PermiMng.Where(x => x.EmpIndex == EmpIndex);
                if (compyPermi.Count() > 0)
                {
                    foreach (var item in compyPermi)
                    {
                        item.Main = false;
                        item.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        item.UpdateBy_Time = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
                foreach (var item in GroupIndex)
                {
                    var HavingPermi = compyPermi.Where(x => x.PermiGpIndex == item);
                    if (HavingPermi.Count() > 0)
                    {
                        foreach (var item2 in HavingPermi)
                        {
                            item2.Main = true;
                            item2.UpdateBy = Convert.ToInt32(User.Identity.Name);
                            item2.UpdateBy_Time = DateTime.Now;
                            db.Entry(item2).State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        db.PermiMng.Add(new PermiMng()
                        {
                            PermiGpIndex = item,
                            EmpIndex = EmpIndex,
                            Main = true,
                            CompanyNo = compyNo,
                            UpdateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy_Time = DateTime.Now,
                            UpdateBy_Time = DateTime.Now,
                        });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Alert = "更新失敗";
            return RedirectToAction("Edit", new { id = EmpIndex });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePermiGroup(int[] empIndex, int[] PermiGroup)
        {
            string compyNo = Session["ComnpanyNo"].ToString();
            if (empIndex == null || PermiGroup == null)
            {
                return Content("請選擇人員及權限群組");
            }
            foreach (int p in PermiGroup)
            {
                foreach (int i in empIndex)
                {
                    var allPermi = db.PermiMng.Where(x => x.EmpIndex == i && x.CompanyNo == compyNo);
                    foreach (var item in allPermi)
                    {
                        item.Main = false;
                        item.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        item.UpdateBy_Time = DateTime.Now;
                        db.Entry(item).State = EntityState.Modified;
                    }
                }
            }
            foreach (int p in PermiGroup)
            {
                foreach (int i in empIndex)
                {
                    var havingPermi = db.PermiMng.Where(x => x.EmpIndex == i && x.PermiGpIndex == p);
                    if (havingPermi.Count() > 0)
                    {
                        var nowpermi = havingPermi.FirstOrDefault();
                        nowpermi.Main = true;
                        nowpermi.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        nowpermi.UpdateBy_Time = DateTime.Now;
                        db.Entry(nowpermi).State = EntityState.Modified;
                    }
                    else
                    {
                        db.PermiMng.Add(new PermiMng()
                        {
                            PermiGpIndex = p,
                            EmpIndex = i,
                            CompanyNo = compyNo,
                            CreateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy_Time = DateTime.Now,
                            UpdateBy = Convert.ToInt32(User.Identity.Name),
                            UpdateBy_Time = DateTime.Now,
                            Main = true
                        });
                    }

                }
            }
            db.SaveChanges();
            return Content("更改成功");
        }

        // GET: PermiMng/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PermiMng permiMng = db.PermiMng.Find(id);
        //    if (permiMng == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(permiMng);
        //}

        // POST: PermiMng/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PermiMng permiMng = db.PermiMng.Find(id);
        //    db.PermiMng.Remove(permiMng);
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
