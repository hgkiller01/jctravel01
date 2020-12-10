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
    public class RestaurantsController : Controller
    {
        AutoCode AC = new AutoCode();
        private TravelContainer db = new TravelContainer();
        int pagesize = 5;

        // GET: Restaurants
        public ActionResult Index(int? Status, int? CityIndex, int? CountryIndexNum, string Ename, string Cname, string Rest_no, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //目前的頁數
            var restaurant = db.Restaurant.OrderBy(x => x.Rest_no).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo == Company);
            if (!string.IsNullOrEmpty(Rest_no))
            {
                ViewBag.Rest_no = Rest_no;
                restaurant = restaurant.Where(x => x.Rest_no.StartsWith(Rest_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                restaurant = restaurant.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                restaurant = restaurant.Where(x => x.Ename.Contains(Ename));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                restaurant = restaurant.Where(x => x.Status == Status);
            }
            if (CityIndex != null)
            {
                ViewBag.CityIndex = CityIndex;
                restaurant = restaurant.Where(x => x.CityIndex == CityIndex);
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
            ViewData["DataCount"] = restaurant.Count();
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            var result = restaurant.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Restaurants/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Restaurant restaurant = db.Restaurant.Find(id);
        //    if (restaurant == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(restaurant.Status);
        //    return View(restaurant);
        //}

        // GET: Restaurants/Create
        public ActionResult Create()
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            var firstCountry = country.First();
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", firstCountry);
            var selectCity = db.City03.Where(x => x.CountryIndex == firstCountry.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname");
            if (selectCity != null)
            {
                var firstCity = selectCity.FirstOrDefault();
                var cityDistrict = db.CityDistrict.Where(x => x.Status == 1 && x.CityIndex == firstCity.CityIndex && x.CompanyNo == Company)
                    .Select(x => new { x.CityDistrictIndex, Cname = x.CityDistrictCode + " " + x.DisCname });
                ViewBag.CityDistrictIndex = new SelectList(cityDistrict, "CityDistrictIndex", "Cname");
            }
            else
            {
                ViewBag.CityDistrictIndex = new SelectList("");
            }
            ViewBag.Status = GetStuatus.GetStatus();//顯示完成選項
            return View();
        }

        // POST: Restaurants/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityDistrictIndex,CountryIndex,CityIndex,ShortName,Ename,Cname,Area,Tele_CountryCode,Tele_Area,Tele1,Tele2,Contact1,Contact1_tele,Contact2,Contact2_tele,Fax,Email,URL,Blog,Longitude,Latitude,Cuisine,Open_Time,Close_Time,ForNight,TimeLimit,Addr,Introduction,Locate_des,Cuisine_Set,Menu,Dayoff,Status,CreateBy,UpdateBy")] Restaurant restaurant)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });//選出已完成國家
            string countryCode = db.Country01.Find(restaurant.CountryIndex).Country_no;
            string cityCode = db.City03.Find(restaurant.CityIndex).City_no;
            restaurant.CreateBy = Convert.ToInt32(User.Identity.Name);
            restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
            try
            {
                if (ModelState.IsValid)
                {
            
                    restaurant.CompanyNo = Company;
                    restaurant.Rest_no = AC.GetAutoCodeR(countryCode, cityCode);
                    restaurant.CreateBy_Time = DateTime.Now;
                    restaurant.UpdateBy_Time = DateTime.Now;
                    db.Restaurant.Add(restaurant);
                    db.SaveChanges();
                    return RedirectToAction("AddTypeTheme", new { id = restaurant.RestIndex });
                }
            }
            catch
            {
                getViewData(restaurant);
                return View(restaurant);
            }
            getViewData(restaurant);
            return View(restaurant);
        }

        public ActionResult UploadImages(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var restaurant = db.Restaurant.Find(id);
            string Company = Session["ComnpanyNo"].ToString();
            if (restaurant.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            Sc_UploadImage su = new Sc_UploadImage();
            su.restIndex = restaurant.RestIndex;
            su.cname = restaurant.Cname;
            su.rest_no = restaurant.Rest_no;
            return View(su);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> imageUpload, string[] imgInfo, int restIndex, string rest_no, string cname)
        {
            string fileName = "";
            string compyNo = Session["ComnpanyNo"].ToString();
            string path = Server.MapPath("~/images/" + compyNo + "/Restaurant");
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
                        img.RestIndex = restIndex;
                        img.ImgInfo = imgInfo[i];
                        img.ImgPath = "/images/" + compyNo + "/Restaurant/" + fileName;
                        img.CreateBy = Convert.ToInt32(User.Identity.Name);
                        img.CreateBy_Time = DateTime.Now;
                        img.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        img.UpdateBy_Time = DateTime.Now;
                        img.POI_Type = 2;
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
                    su.restIndex = restIndex;
                    su.cname = cname;
                    su.rest_no = rest_no;
                    ViewBag.success = "上傳成功";
                    return View(su);
                }
                catch
                {
                    su.restIndex = restIndex;
                    su.cname = cname;
                    su.rest_no = rest_no;
                    ViewBag.success = "上傳失敗";
                    return View(su);
                }
                   
            }
            su.restIndex = restIndex;
            su.cname = cname;
            su.rest_no = rest_no;
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

        public ActionResult SelectPage(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var restaurant = db.Restaurant.Find(id);
            return View(restaurant);
        }
        public void getViewData(Restaurant restaurant)
        {
            string Company = Session["ComnpanyNo"].ToString();
            ViewBag.CreateBy = db.HRInfo.Find(restaurant.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(restaurant.UpdateBy).EmpName;
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", restaurant.CountryIndex);
            var selectCity = db.City03.Where(x => x.CountryIndex == restaurant.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname");
            if (selectCity != null)
            {
                var cityDistrict = db.CityDistrict.Where(x => x.Status == 1 && x.CityIndex == restaurant.CityIndex && x.CompanyNo == Company)
                    .Select(x => new { x.CityDistrictIndex, Cname = x.CityDistrictCode + " " + x.DisCname });
                ViewBag.CityDistrictIndex = new SelectList(cityDistrict, "CityDistrictIndex", "Cname", restaurant.CityDistrictIndex);
            }
            else
            {
                ViewBag.CityDistrictIndex = new SelectList("");
            }

            ViewBag.Status = GetStuatus.GetStatus(restaurant.Status);//顯示選擇狀態
        }

        // GET: Restaurants/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (restaurant.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.City_no = restaurant.City03.City_no;
            ViewBag.Country_no = restaurant.Country01.Country_no;
            getViewData(restaurant);
            ViewBag.Status = GetStuatus.GetStatus(restaurant.Status);//顯示選擇狀態
            return View(restaurant);
        }
        public ActionResult AddTypeTheme(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
       
            var restaurant = db.Restaurant.Find(id);
            if (restaurant.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> TypeIndex = new List<int>();
            List<int> ThemeIndex = new List<int>();
            if (restaurant.Res_Theme.Count() > 0)
            {
                foreach (var item in restaurant.Res_Theme)
                {
                    if (item.Main == true)
                    {
                        ThemeIndex.Add(item.Theme_index);
                    }
                }
            }
            if (restaurant.Res_Type.Count() > 0)
            {
                foreach (var item in restaurant.Res_Type)
                {
                    if (item.Main == true)
                    {
                        TypeIndex.Add(item.Type_index);
                    }
                }
            }
            TypeThemeRViewModel typeTheme = new TypeThemeRViewModel(Company, restaurant.RestIndex, TypeIndex.ToArray(), ThemeIndex.ToArray());

            return View(typeTheme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTypeTheme(TypeThemeRViewModel typeTheme)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatRest1 = db.Res_Theme.Where(x => x.RestIndex == typeTheme.restIndex && x.CompanyNo == Company);
            var repeatRest1 = db.Res_Type.Where(x => x.RestIndex == typeTheme.restIndex && x.CompanyNo == Company);
            foreach (var item in RepeatRest1)
            {
                var restaurant = db.Res_Theme.Find(item.Res_ThemeIndex);
                restaurant.Main = false;
                restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                restaurant.UpdateBy_Time = DateTime.Now;
                db.Entry(restaurant).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatRest1)
            {
                var restaurant = db.Res_Type.Find(item.Res_TypeIndex);
                restaurant.Main = false;
                restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                restaurant.UpdateBy_Time = DateTime.Now;
                db.Entry(restaurant).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (typeTheme.Theme_index != null)
            {
                foreach (int i in typeTheme.Theme_index)
                {
                    Res_Theme rt = new Res_Theme()
                    {
                        RestIndex = typeTheme.restIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Theme_index = i
                    };
                    var RepeatRest = db.Res_Theme.Where(x => x.Theme_index == i && x.RestIndex == rt.RestIndex && x.CompanyNo == Company);
                    if (RepeatRest.Count() > 0)
                    {
                        var restaurant = RepeatRest.FirstOrDefault();
                        restaurant.Main = true;
                        restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        restaurant.UpdateBy_Time = DateTime.Now;
                        db.Entry(restaurant).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.theme1.Add(rt);
                    }

                }
            }

            if (typeTheme.Type_index != null)
            {
                foreach (int i in typeTheme.Type_index)
                {
                    Res_Type rt = new Res_Type()
                    {
                        RestIndex = typeTheme.restIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Type_index = i
                    };
                    var repeatRest = db.Res_Type.Where(x => x.Type_index == i && x.RestIndex == rt.RestIndex && x.CompanyNo == Company);
                    if (repeatRest.Count() > 0)
                    {
                        var restaurant = repeatRest.FirstOrDefault();
                        restaurant.Main = true;
                        restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        restaurant.UpdateBy_Time = DateTime.Now;
                        db.Entry(restaurant).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.type1.Add(rt);
                    }
                }
            }
            try
            {
                db.Res_Theme.AddRange(typeTheme.theme1);
                db.Res_Type.AddRange(typeTheme.type1);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddTypeTheme", new { id = typeTheme.restIndex });
            }

            TempData["Success"] = "更改成功";
            return RedirectToAction("AddTypeTheme", new { id = typeTheme.restIndex });
        }
        public ActionResult AddDiningAmos(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var restaurant = db.Restaurant.Find(id);
            if (restaurant.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> amosph_Index = new List<int>();
            List<int> diningSty_Index = new List<int>();
            if (restaurant.ResAmos.Count() > 0)
            {
                foreach (var item in restaurant.ResAmos)
                {
                    if (item.Main == true)
                    {
                        amosph_Index.Add(item.Amosph_Index);
                    }
                }
            }
            if (restaurant.ResDining.Count() > 0)
            {
                foreach (var item in restaurant.ResDining)
                {
                    if (item.Main == true)
                    {
                        diningSty_Index.Add(item.DiningSty_Index);
                    }
                }
            }
            DiningAmosViewModel diningAmos = new DiningAmosViewModel(Company, restaurant.RestIndex, amosph_Index.ToArray(), diningSty_Index.ToArray());

            return View(diningAmos);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddDiningAmos(DiningAmosViewModel diningAmos)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatRest1 = db.ResAmos.Where(x => x.RestIndex == diningAmos.restIndex && x.CompanyNo == Company);
            var repeatRest1 = db.ResDining.Where(x => x.RestIndex == diningAmos.restIndex && x.CompanyNo == Company);
            foreach (var item in RepeatRest1)
            {
                var restaurant = db.ResAmos.Find(item.ResAmosIndex);
                restaurant.Main = false;
                restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                restaurant.UpdateBy_Time = DateTime.Now;
                db.Entry(restaurant).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatRest1)
            {
                var restaurant = db.ResDining.Find(item.ResDiningIndex);
                restaurant.Main = false;
                restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                restaurant.UpdateBy_Time = DateTime.Now;
                db.Entry(restaurant).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (diningAmos.amosph_index != null)
            {
                foreach (int i in diningAmos.amosph_index)
                {
                    ResAmos ra = new ResAmos()
                    {
                        RestIndex = diningAmos.restIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = diningAmos.CompanyNo,
                        Amosph_Index = i
                    };
                    var RepeatRest = db.ResAmos.Where(x => x.ResAmosIndex == i && x.RestIndex == ra.RestIndex && x.CompanyNo == Company);
                    if (RepeatRest.Count() > 0)
                    {
                        var restaurant = RepeatRest.FirstOrDefault();
                        restaurant.Main = true;
                        restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        restaurant.UpdateBy_Time = DateTime.Now;
                        db.Entry(restaurant).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        diningAmos.resAmos.Add(ra);
                    }

                }
            }

            if (diningAmos.diningSty_index != null)
            {
                foreach (int i in diningAmos.diningSty_index)
                {
                    ResDining rd = new ResDining()
                    {
                        RestIndex = diningAmos.restIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = diningAmos.CompanyNo,
                        DiningSty_Index = i
                    };
                    var repeatRest = db.ResDining.Where(x => x.DiningSty_Index == i && x.RestIndex == rd.RestIndex && x.CompanyNo == Company);
                    if (repeatRest.Count() > 0)
                    {
                        var restaurant = repeatRest.FirstOrDefault();
                        restaurant.Main = true;
                        restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        restaurant.UpdateBy_Time = DateTime.Now;
                        db.Entry(restaurant).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        diningAmos.resDining.Add(rd);
                    }
                }
            }
            try
            {
                db.ResAmos.AddRange(diningAmos.resAmos);
                db.ResDining.AddRange(diningAmos.resDining);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddDiningAmos", new { id = diningAmos.restIndex });
            }
            TempData["Success"] = "更改成功";
            return RedirectToAction("AddDiningAmos", new {id=diningAmos.restIndex });
        }



        // POST: Restaurants/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityDistrictIndex,CompanyNo,Rest_no,RestIndex,CountryIndex,CityIndex,ShortName,Ename,Cname,Area,Tele_CountryCode,Tele_Area,Tele1,Tele2,Contact1,Contact1_tele,Contact2,Contact2_tele,Fax,Email,URL,Blog,Longitude,Latitude,Cuisine,Open_Time,Close_Time,ForNight,TimeLimit,Addr,Introduction,Locate_des,Cuisine_Set,Menu,Dayoff,Status,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] Restaurant restaurant)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (ModelState.IsValid)
            {
                restaurant.UpdateBy_Time = DateTime.Now;
                restaurant.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var restaurant2 = db.Restaurant.Find(restaurant.RestIndex);
            restaurant.Country01 = restaurant2.Country01;
            restaurant.City03 = restaurant2.City03;
            getViewData(restaurant);
            ViewBag.Status = GetStuatus.GetStatus(restaurant.Status);//顯示選擇狀態
            return View(restaurant);
        }

        // GET: Restaurants/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurant.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (restaurant.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            ViewBag.ShowStuatus = GetStuatus.ValidaStatus(restaurant.Status);
            return View(restaurant);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Restaurant restaurant = db.Restaurant.Find(id);
            restaurant.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)//以Ajax取得城市資料
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company);
            var NowCity2 = NowCity.Select(x => new { x.CityIndex, x.Cname,x.City_no });
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
