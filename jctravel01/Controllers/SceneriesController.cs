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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeCompny]
    public class SceneriesController : Controller
    {
        private TravelContainer db = new TravelContainer();
        AutoCode Ac = new AutoCode();
        private int pagesize = 5;

        // GET: Sceneries
        public ActionResult Index(int? Status, int? CityIndex, int? CountryIndexNum, string Ename, string Cname, string Scenery_no, int page = 1)
        {
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            string Company = Session["ComnpanyNo"].ToString();
            var scenery = db.Scenery.OrderBy(x => x.Scenery_no).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Scenery_no))
            {
                ViewBag.Scenery_no = Scenery_no;
                scenery = scenery.Where(x => x.Scenery_no.StartsWith(Scenery_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                scenery = scenery.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                scenery = scenery.Where(x => x.Ename.Contains(Ename));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                scenery = scenery.Where(x => x.Status == Status);
            }
            if(CityIndex !=null)
            {
                ViewBag.CityIndex = CityIndex;
                scenery = scenery.Where(x => x.CityIndex == CityIndex);
            }
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var firstCountry = country.FirstOrDefault();
            if (CountryIndexNum == null)
            {
                ViewBag.CountryIndexNum = new SelectList(country, "CountryIndex", "Cname", firstCountry);
                ViewBag.CityIndexNum = new SelectList("");
            }
            else
            {
                ViewBag.CountryIndexNum = new SelectList(country, "CountryIndex", "Cname", CountryIndexNum);
                var selectCity = db.City03.Where(x => x.CountryIndex == CountryIndexNum && x.Status == 1 && x.CompanyNo == Company)
                    .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
                ViewBag.CityIndexNum = new SelectList(selectCity, "CityIndex", "Cname", CityIndex);
            }
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            ViewBag.StatusNum = GetStuatus.DefualStatus();            
            ViewData["DataCount"] = scenery.Count();           
            var restult = scenery.ToPagedList(CurrentPage, pagesize);
            return View(restult);
        }

        // GET: Sceneries/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Scenery scenery = db.Scenery.Find(id);
        //    if (scenery == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowSafeLevel = GetSafeLevel.GetsafeLevel (scenery.SafeLevel);
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(scenery.Status);
        //    return View(scenery);
        //}

        // GET: Sceneries/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new{x.CountryIndex, Cname = x.Country_no+" "+x.Cname});
            var firstCountry = country.FirstOrDefault();
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", firstCountry);
            var selectCity = db.City03.Where(x => x.CountryIndex == firstCountry.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname");
            if (selectCity != null)
            {
                var firstCity = selectCity.FirstOrDefault();
                var cityDistrict = db.CityDistrict.Where(x => x.Status == 1 &&x.CityIndex == firstCity.CityIndex && x.CompanyNo ==Company)
                    .Select(x => new{x.CityDistrictIndex, Cname = x.CityDistrictCode+" "+x.DisCname});
                ViewBag.CityDistrictIndex = new SelectList(cityDistrict, "CityDistrictIndex", "Cname");
            }
            else
            {
                ViewBag.CityDistrictIndex = new SelectList("");
            }
            ViewBag.SafeLevel = GetSafeLevel.safelevel();//取得安全層級
            ViewBag.Status = GetStuatus.GetStatus();
            return View();
        }

        // POST: Sceneries/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CountryIndex,CityIndex,ShortName,Ename,Cname,CityDistrictIndex,Tele_CountryCode,Tele_Area,Tele_number,Fax,Email,URL,Blog,Longitude,Latitude,Addr,Introduction,Fee,Locate_des,Found_Date,Stay_Time,Bulit_Area,Suggestion,Start_time,LastCall,Close_time,TimeSheet,Dayoff,ForNight,SafeLevel,MainPoint,Status,CreateBy,UpdateBy")] Scenery scenery)
        {
            string Company = Session["ComnpanyNo"].ToString();
            string countryCode = db.Country01.Find(scenery.CountryIndex).Country_no;
            string cityCode = db.City03.Find(scenery.CityIndex).City_no;
            scenery.UpdateBy = Convert.ToInt32(User.Identity.Name);
            scenery.CreateBy = Convert.ToInt32(User.Identity.Name);
            scenery.UpdateBy_Time = DateTime.Now;
            scenery.CreateBy_Time = DateTime.Now;
            try
            {
                if (ModelState.IsValid)
                {
                    scenery.CompanyNo = Company;
                    scenery.Scenery_no = Ac.GetAutoCodeP(countryCode,cityCode);
                    db.Scenery.Add(scenery);
                    db.SaveChanges();
                    return RedirectToAction("AddTypeTheme", new { id = scenery.Scenery_index });
                }
            }
            catch
            {   
                getViewdata(scenery);
                return View(scenery);
            }
            getViewdata(scenery);
            return View(scenery);
        }
        public ActionResult UploadImages(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var scenery = db.Scenery.Find(id);
            string Company = Session["ComnpanyNo"].ToString();
            if (scenery.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            Sc_UploadImage su = new Sc_UploadImage();
            su.scenery_index = scenery.Scenery_index;
            su.cname = scenery.Cname;
            su.scenery_no = scenery.Scenery_no;
            return View(su);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> imageUpload, string[] imgInfo, int scenery_index, string scenery_no, string cname)
        {
            string fileName = "";
            string compyNo = Session["ComnpanyNo"].ToString();
            string path = Server.MapPath("~/images/" + compyNo + "/Scenery");
            Sc_UploadImage su = new Sc_UploadImage();
            foreach (string info in imgInfo)
            {
                if (string.IsNullOrEmpty(info))
                {
                    ModelState.AddModelError("imgInfo", "圖片說明不可為空白");
                    break;
                }
                if (imgInfo.Length > 50)
                {
                    ModelState.AddModelError("imgInfo", "圖片說明字數不可超過50字");
                    break;
                }
            }
            foreach (var item in imageUpload)
            {
                if (item != null)
                {
                    if (item.ContentLength > 0)
                    {
                        string fileExten = item.ContentType;
                        if (fileExten != "image/jpg" && fileExten != "image/jpeg" && fileExten != "image/png")
                        {
                            ModelState.AddModelError("imageUpload", "只能上傳JPG和PNG");
                            break;
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("imageUpload", "檔案大小不正確");
                        break;
                    }
                }
                else
                {
                    ModelState.AddModelError("imageUpload", "請選擇圖片");
                    break;
                }

            }
            if (ModelState.IsValid)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var imageupload = imageUpload.ToArray();
                try
                {
                    for (int i = 0; i < imageupload.Count(); i++)
                    {
                        Random rm = new Random();
                        string fileExton = Path.GetExtension(imageupload[i].FileName);
                        fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + rm.Next(9) + fileExton;
                        var path2 = Path.Combine(path, fileName);
                        imageupload[i].SaveAs(path2);
                        Img img = new Img();
                        img.Scenery_index = scenery_index;
                        img.ImgInfo = imgInfo[i];
                        img.ImgPath = "/images/" + compyNo + "/Scenery/" + fileName;
                        img.CreateBy = Convert.ToInt32(User.Identity.Name);
                        img.CreateBy_Time = DateTime.Now;
                        img.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        img.UpdateBy_Time = DateTime.Now;
                        img.POI_Type = 1;
                        img.OrderNo = 1;
                        img.CompanyNo = compyNo;
                        img.Status = 1;
                        Image waterImage = Image.FromFile(Server.MapPath("~/images/WaterMark.PNG"));
                        Image pastImage = GetImageFromFile(path2);
                        Bitmap Past = new Bitmap(pastImage);
                        ImageFormat thisFormat = pastImage.RawFormat;
                        Graphics gra = Graphics.FromImage(Past);
                        gra.DrawImage(waterImage, new Rectangle(Past.Width - waterImage.Width, Past.Height - waterImage.Height, Past.Width, Past.Height)
                            , 0, 0, Past.Width, Past.Height, GraphicsUnit.Pixel);
                        Past.Save(path2, thisFormat);
                        pastImage.Dispose();
                        gra.Dispose();
                        db.Img.Add(img);
                        db.SaveChanges();

                    }
                    ViewBag.success = "上傳成功";
                    su.scenery_index = scenery_index;
                    su.cname = cname;
                    su.scenery_no = scenery_no;
                    return View(su);
                }
                catch
                {
                    su = new Sc_UploadImage();
                    su.scenery_index = scenery_index;
                    su.cname = cname;
                    su.scenery_no = scenery_no;
                    ViewBag.success = "上傳失敗";
                    return View(su);
                }              
            }          
            su = new Sc_UploadImage();
            su.scenery_index = scenery_index;
            su.cname = cname;
            su.scenery_no = scenery_no;
            ViewBag.success = "上傳失敗";
            return View(su);
        }
        public Image GetImageFromFile(string path)
        {
            Image image = null;

            using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
            {
                image = Image.FromStream(fs);
                fs.Close();
            }
            return image;
        }

        // GET: Sceneries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenery scenery = db.Scenery.Find(id);
            if (scenery == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (scenery.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            getViewdata(scenery);
            return View(scenery);
        }

        // POST: Sceneries/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Scenery_no,Scenery_index,CompanyNo,CountryIndex,CityIndex,City_no,ShortName,Ename,Cname,CityDistrictIndex,Tele_CountryCode,Tele_Area,Tele_number,Fax,Email,URL,Blog,Longitude,Latitude,Addr,Introduction,Fee,Locate_des,Found_Date,Stay_Time,Bulit_Area,Suggestion,Start_time,LastCall,Close_time,TimeSheet,Dayoff,ForNight,SafeLevel,MainPoint,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] Scenery scenery)
        {
            if (ModelState.IsValid)
            {
                scenery.UpdateBy_Time = DateTime.Now;
                scenery.UpdateBy = 1;
                db.Entry(scenery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            Scenery scenery2 = db.Scenery.Find(scenery.Scenery_index);
            scenery.City03 = scenery2.City03;
            scenery.Country01 = scenery2.Country01;
            scenery.Scenery_Theme = scenery2.Scenery_Theme;
            scenery.Scenery_Type = scenery2.Scenery_Type;
            getViewdata(scenery);
            return View(scenery2);
        }
        public void getViewdata(Scenery scenery)
        {
            string Company = Session["ComnpanyNo"].ToString();
            ViewBag.CreateBy = db.HRInfo.Find(scenery.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(scenery.UpdateBy).EmpName;
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", scenery.CountryIndex);
            var selectCity = db.City03.Where(x => x.CountryIndex == scenery.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname",scenery.CityIndex);
            if (selectCity != null)
            {
                var cityDistrict = db.CityDistrict.Where(x => x.Status == 1 && x.CityIndex == scenery.CityIndex && x.CompanyNo == Company)
                    .Select(x => new { x.CityDistrictIndex, Cname = x.CityDistrictCode + " " + x.DisCname });
                ViewBag.CityDistrictIndex = new SelectList(cityDistrict, "CityDistrictIndex", "Cname",scenery.CityDistrictIndex);
            }
            else
            {
                ViewBag.CityDistrictIndex = new SelectList("");
            }
            ViewBag.SafeLevel = GetSafeLevel.safelevel(scenery.SafeLevel);//取得安全層級
            ViewBag.Status = GetStuatus.GetStatus(scenery.Status);
        }

        // GET: Sceneries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scenery scenery = db.Scenery.Find(id);
            if (scenery == null)
            {
                return HttpNotFound();
            }
            ViewBag.ShowSafeLevel = GetSafeLevel.GetsafeLevel(scenery.SafeLevel);
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(scenery.Status);
            return View(scenery);
        }

        // POST: Sceneries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Scenery scenery = db.Scenery.Find(id);
            scenery.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo==Company);
            var NowCity2 = NowCity.Select(x => new {x.CityIndex ,x.City_no, x.Cname});
            return Json(NowCity2);
        }
        [HttpPost]
        public ActionResult GetCityDistrictCode(int CityIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCode = db.CityDistrict.Where(x => x.Status == 1 && x.CityIndex == CityIndex && x.CompanyNo == Company);
            var NowCode2 = NowCode.Select(x => new { x.CityDistrictIndex, x.CityDistrictCode, x.DisCname });
            return Json(NowCode2);
        }
        public ActionResult AddTypeTheme(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var scenery = db.Scenery.Find(id);
            if (scenery.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> TypeIndex = new List<int>();
            List<int> ThemeIndex = new List<int>();
            if (scenery.Scenery_Theme.Count() > 0)
            {
                foreach (var item in scenery.Scenery_Theme)
                {
                    if (item.Main == true)
                    {
                        ThemeIndex.Add(item.Theme_index);
                    } 
                }
            }
            if (scenery.Scenery_Type.Count() > 0)
            {
                foreach (var item in scenery.Scenery_Type)
                {
                    if (item.Main == true)
                    {
                        TypeIndex.Add(item.Type_index);
                    }
                }
            }            
            TypeThemeViewModel typeTheme = new TypeThemeViewModel(Company,scenery.Scenery_index,TypeIndex.ToArray(),ThemeIndex.ToArray());
            
            return View(typeTheme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTypeTheme(TypeThemeViewModel typeTheme)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatScenery1 = db.Scenery_Theme.Where(x => x.Scenery_index == typeTheme.scenery_index && x.CompanyNo == Company);
            var repeatScenery1 = db.Scenery_Type.Where(x => x.Scenery_index == typeTheme.scenery_index && x.CompanyNo == Company);
            foreach (var item in RepeatScenery1)
            {
                    var scenery = db.Scenery_Theme.Find(item.Scenery_ThemeIndex);
                    scenery.Main = false;
                    scenery.UpdateBy = Convert.ToInt32(User.Identity.Name);
                    scenery.UpdateBy_Time = DateTime.Now;
                    db.Entry(scenery).State = EntityState.Modified;
                    //db.SaveChanges();
            }
            foreach (var item in repeatScenery1)
            {
                    var scenery = db.Scenery_Type.Find(item.Scenery_TypeIndex);
                    scenery.Main = false;
                    scenery.UpdateBy = Convert.ToInt32(User.Identity.Name);
                    scenery.UpdateBy_Time = DateTime.Now;
                    db.Entry(scenery).State = EntityState.Modified;
                    //db.SaveChanges();
            }
            if (typeTheme.Theme_index != null)
            {
                foreach (int i in typeTheme.Theme_index)
                {
                    Scenery_Theme st = new Scenery_Theme()
                    {
                        Scenery_index = typeTheme.scenery_index,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Theme_index = i
                    };
                    var RepeatScenery = db.Scenery_Theme.Where(x => x.Theme_index == i && x.Scenery_index == st.Scenery_index && x.CompanyNo == Company);
                    if (RepeatScenery.Count() > 0)
                    {
                        var scenery = RepeatScenery.FirstOrDefault();
                        scenery.Main = true;
                        scenery.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        scenery.UpdateBy_Time = DateTime.Now;
                        db.Entry(scenery).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.theme1.Add(st);
                    }

                }
            }

            if (typeTheme.Type_index != null)
            {
                foreach (int i in typeTheme.Type_index)
                {
                    Scenery_Type st = new Scenery_Type()
                    {
                        Scenery_index = typeTheme.scenery_index,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Type_index = i
                    };
                    var repeatScenery = db.Scenery_Type.Where(x => x.Type_index == i && x.Scenery_index == st.Scenery_index && x.CompanyNo == Company);
                    if (repeatScenery.Count() > 0)
                    {
                        var scenery = repeatScenery.FirstOrDefault();
                        scenery.Main = true;
                        scenery.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        scenery.UpdateBy_Time = DateTime.Now;
                        db.Entry(scenery).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.type1.Add(st);
                    }
                }
            }
            try
            {
                db.Scenery_Theme.AddRange(typeTheme.theme1);
                db.Scenery_Type.AddRange(typeTheme.type1);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更新失敗";
                return RedirectToAction("AddTypeTheme", new { id = typeTheme.scenery_index });
            }
            TempData["Success"] = "更新成功";
            return RedirectToAction("AddTypeTheme", new {id =typeTheme.scenery_index });
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
