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
using MoreLinq;

namespace jctravel01.Controllers
{
    [AuthorizeCompny]
    [Authorize]
    public class PermiGruopsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: PermiGruops
        public ActionResult Index(string PermiGpNo, string PermiGpName, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            //var permiGruop = db.PermiGruop.Where(x => x.CompanyNo == Company).GroupBy(x => new { x.PermiGpNo, x.PermiGpName }).Select(x => new { x.Key.PermiGpNo, x.Key.PermiGpName });
            IEnumerable<PermiGruop> permiGruop = db.PermiGruop.Where(x => x.CompanyNo == Company).OrderBy(x => x.PermiGpNo);
            if (!string.IsNullOrEmpty(PermiGpNo))
            {
                permiGruop = permiGruop.Where(x => x.PermiGpNo.StartsWith(PermiGpNo));
            }
            if (!string.IsNullOrEmpty(PermiGpName))
            {
                permiGruop = permiGruop.Where(x => x.PermiGpName.Contains(PermiGpName));
            }
            //List<PermiGruopViewModel> PGVM = new List<PermiGruopViewModel>();
            //foreach (var item in permiGruop)
            //{
            //    PGVM.Add(new PermiGruopViewModel()
            //    {
            //        PermiGpName = item.PermiGpName,
            //        PermiGpNo = item.PermiGpNo
            //    });
            //}
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            ViewData["DataCount"] = permiGruop.Count();
            var result = permiGruop.ToPagedList(CurrentPage, pagesize);
           return View(result);
        }

        // GET: PermiGruops/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PermiGruop permiGruop = db.PermiGruop.Find(id);
        //    if (permiGruop == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(permiGruop);
        //}

        // GET: PermiGruops/Create
        public ActionResult Create()
        {
            PermiGruopCreateViewModel createModel = new PermiGruopCreateViewModel();
            //將顯示權限分類
            var PoiPermi = db.PermiIndex.Where(x => x.MainPerNo == "01");
            foreach (var item in PoiPermi)
            {
                createModel.POIPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var TourPermi = db.PermiIndex.Where(x => x.MainPerNo == "02");
            foreach (var item in TourPermi)
            {
                createModel.TourPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var SalesPermi = db.PermiIndex.Where(x => x.MainPerNo == "03");
            foreach (var item in SalesPermi)
            {
                createModel.SalesPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var EmpPermi = db.PermiIndex.Where(x => x.MainPerNo == "04");
            foreach (var item in EmpPermi)
            {
                createModel.EmpPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var PermiPermi = db.PermiIndex.Where(x => x.MainPerNo == "05");
            foreach (var item in PermiPermi)
            {
                createModel.PermiPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var FrontEndPermi = db.PermiIndex.Where(x => x.MainPerNo == "06");
            foreach (var item in FrontEndPermi)
            {
                createModel.FrontEndPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var BasicPermi = db.PermiIndex.Where(x => x.MainPerNo == "07");
            foreach (var item in BasicPermi)
            {
                createModel.BasicPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            var AccountPermi = db.PermiIndex.Where(x => x.MainPerNo == "08");
            foreach (var item in AccountPermi)
            {
                createModel.AccountPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
            }
            return View(createModel);
        }

        // POST: PermiGruops/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Permi[] permi, PermiGruopViewModel PGV)
        {
            List<Permi> NowPermi = new List<Permi>();
            foreach (var item in permi)
            {
                if (item.PermiIndex != null)
                {
                    NowPermi.Add(item);
                }
            }
            string Company = Session["ComnpanyNo"].ToString();
            //找出是否有此群組代號
            var groupno = db.PermiGruop.Where(x => x.CompanyNo == Company && x.PermiGpNo == PGV.PermiGpNo);
            if (groupno.Count() >0)
            {
                ModelState.AddModelError("PermiGpNo", "群組代號重複!");
            }
            if (ModelState.IsValid)
            {
                //加入權限群組
                PermiGruop permi2 = new PermiGruop()
                {
                    CompanyNo = Company,
                    Descri = PGV.Descri,
                    PermiGpName = PGV.PermiGpName,
                    CreateBy = Convert.ToInt32(User.Identity.Name),
                    CreateBy_Time = DateTime.Now,
                    UpdateBy = Convert.ToInt32(User.Identity.Name),
                    UpdateBy_Time = DateTime.Now,
                    PermiGpNo = PGV.PermiGpNo
                };
                db.PermiGruop.Add(permi2);
                //加入功能權限對應表
                foreach (var item in NowPermi)
                {
                    if (item.SharePermi)
                    {
                        item.ReadPermi = true;
                    }
                    db.Authorze_index.Add(new Authorze_index()
                    {
                        permiGpIndex = permi2.PermiGpIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        PermiIndex = Convert.ToInt32(item.PermiIndex),
                        ChPermi = item.ChPermi,
                        ReadPermi = item.ReadPermi,
                        SharePermi = item.SharePermi
                    });
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PermiGruopCreateViewModel createModel = getViewData(NowPermi, PGV);
            return View(createModel);
        }
        public PermiGruopCreateViewModel getViewData(IEnumerable<Permi> NowPermi, PermiGruopViewModel PGV)
        {
            PermiGruopCreateViewModel createModel = new PermiGruopCreateViewModel();
            createModel.PermiGpNo = PGV.PermiGpNo;
            createModel.PermiGpName = PGV.PermiGpName;
            createModel.Descri = PGV.Descri;
            //分類並將所有已經選擇的項目列出
            var PoiPermi = db.PermiIndex.Where(x => x.MainPerNo == "01");
            foreach (var item in PoiPermi)
            {
                createModel.POIPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.POIPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var TourPermi = db.PermiIndex.Where(x => x.MainPerNo == "02");
            foreach (var item in TourPermi)
            {
                createModel.TourPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.TourPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var SalesPermi = db.PermiIndex.Where(x => x.MainPerNo == "03");
            foreach (var item in SalesPermi)
            {
                createModel.SalesPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.SalesPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var EmpPermi = db.PermiIndex.Where(x => x.MainPerNo == "04");
            foreach (var item in EmpPermi)
            {
                createModel.EmpPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.EmpPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var PermiPermi = db.PermiIndex.Where(x => x.MainPerNo == "05");
            foreach (var item in PermiPermi)
            {
                createModel.PermiPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.PermiPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var FrontEndPermi = db.PermiIndex.Where(x => x.MainPerNo == "06");
            foreach (var item in FrontEndPermi)
            {
                createModel.FrontEndPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.FrontEndPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var BasicPermi = db.PermiIndex.Where(x => x.MainPerNo == "07");
            foreach (var item in BasicPermi)
            {
                createModel.BasicPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.BasicPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            var AccountPermi = db.PermiIndex.Where(x => x.MainPerNo == "08");
            foreach (var item in AccountPermi)
            {
                createModel.AccountPermi.Add(new Permi() { MainPerNo = item.MainPerNo, AltPerName = item.AltPerName, PermiIndex = item.Permilindex, PermiNo = item.PermiNo });
                foreach (var item2 in NowPermi)
                {
                    var poi = createModel.AccountPermi.Where(x => x.PermiIndex == item2.PermiIndex).FirstOrDefault();
                    if (poi != null)
                    {
                        poi.ChPermi = item2.ChPermi;
                        poi.ReadPermi = item2.ReadPermi;
                        poi.SharePermi = item2.SharePermi;
                    }
                }
            }
            return createModel;
        }

        // GET: PermiGruops/Edit/5
        public ActionResult Edit(string id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //找出是否有此群組對應表
         var authorze_Index = db.Authorze_index.Where(x => x.PermiGruop.PermiGpNo == id && x.PermiGruop.CompanyNo == Company);
            if (authorze_Index == null)
            {
                return HttpNotFound();
            }
            List<Permi> nowPermi = new List<Permi>();
            //將所有對應表的對應讀出
            foreach (var item in authorze_Index)
            {
                 nowPermi.Add(new Permi()
                 {
                     PermiIndex = item.PermiIndex,
                     AltPerName = item.PermiIndex1.AltPerName,
                     MainPerNo = item.PermiIndex1.MainPerNo,
                     ChPermi = item.ChPermi,
                     ReadPermi = item.ReadPermi,
                     SharePermi = item.SharePermi
                 });
            }
            PermiGruopViewModel PGV = new PermiGruopViewModel();
            var permiGruop = db.PermiGruop.Where(x => x.PermiGpNo == id && x.CompanyNo == Company);
            //顯示群組名稱,代號,說明
            PGV.PermiGpNo = permiGruop.FirstOrDefault().PermiGpNo;
            PGV.PermiGpName = permiGruop.FirstOrDefault().PermiGpName;
            PGV.Descri = permiGruop.FirstOrDefault().Descri;
            //丟入資料傳回顯示
          PermiGruopCreateViewModel createModel = getViewData(nowPermi.AsEnumerable(), PGV);
          return View(createModel);
        }

        // POST: PermiGruops/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Permi[] permi, PermiGruopViewModel PGV)
        {
            if (ModelState.IsValid)
            {
                string Company = Session["ComnpanyNo"].ToString();
                List<Permi> NowPermi = new List<Permi>();
                //挑出有選擇的權限index
                foreach (var item in permi)
                {
                    if (item.PermiIndex != null)
                    {
                        NowPermi.Add(item);
                    }
                }
                //找出權限群組
                var PermiHaving = db.PermiGruop.Where(x => x.PermiGpNo == PGV.PermiGpNo && x.CompanyNo == Company).FirstOrDefault();
                if (PermiHaving != null)
                {
                    //重置此群組所有權限
                    foreach (var item in PermiHaving.Authorze_index)
                      {
                          item.ChPermi = false;
                          item.ReadPermi = false;
                          item.SharePermi = false;
                          item.UpdateBy = Convert.ToInt32(User.Identity.Name);
                          item.UpdateBy_Time = DateTime.Now;
                          db.Entry(item).State = EntityState.Modified;
                      }
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
               
                foreach (var item in NowPermi)
                {
                    if (item.SharePermi)
                    {
                        item.ReadPermi = true;
                    }
                    //將所有群組內的功能權限變更設定
                    foreach (var item2 in PermiHaving.Authorze_index)
                    {
                        if (item.PermiIndex == item2.PermiIndex)
                        {
                            item2.ReadPermi = item.ReadPermi;
                            item2.ChPermi = item.ChPermi;
                            item2.SharePermi = item.SharePermi;
                            item2.UpdateBy = Convert.ToInt32(User.Identity.Name);
                            item2.UpdateBy_Time = DateTime.Now;
                            db.Entry(item2).State = EntityState.Modified;
                        }
                    }
                   
                    //TODO 今日進度
                }

                foreach (var item in NowPermi)
                {
                    if (item.SharePermi)
                    {
                        item.ReadPermi = true;
                    }
                    //加入有選擇的權限,但對應表內沒有的
                    var authorze_index = db.Authorze_index.Where(x => x.PermiIndex == item.PermiIndex && x.PermiGruop.PermiGpIndex == PermiHaving.PermiGpIndex);
                    if (authorze_index.Count() == 0)
                    {
                        db.Authorze_index.Add(new Authorze_index()
                        {
                            ReadPermi = item.ReadPermi,
                            ChPermi = item.ChPermi,
                            SharePermi = item.SharePermi,
                            permiGpIndex = PermiHaving.PermiGpIndex,
                            PermiIndex = Convert.ToInt32(item.PermiIndex),
                            CreateBy = Convert.ToInt32(User.Identity.Name),
                            UpdateBy = Convert.ToInt32(User.Identity.Name),
                            CreateBy_Time = DateTime.Now,
                            UpdateBy_Time = DateTime.Now
                        });
                    }
                          
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getViewData(permi.AsEnumerable(), PGV);
            return View();
        }

        // GET: PermiGruops/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    PermiGruop permiGruop = db.PermiGruop.Find(id);
        //    if (permiGruop == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(permiGruop);
        //}

        //// POST: PermiGruops/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    PermiGruop permiGruop = db.PermiGruop.Find(id);
        //    db.PermiGruop.Remove(permiGruop);
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
