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
    public class HotelsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        Star sr = new Star();
        private int pagesize = 5;
        // GET: Hotels
        public ActionResult Index(int? Status, int? CityIndex, int? CountryIndexNum, string Ename, string Cname, string Holtel_no, int page = 1)
        {
            string Company = Session["ComnpanyNo"].ToString();
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            var hotel = db.Hotel.OrderBy(x => x.Holtel_no).Where(x => (x.Status == 1 || x.Status == 2) && x.CompanyNo ==Company);
            if (!string.IsNullOrEmpty(Holtel_no))
            {
                ViewBag.Holtel_no = Holtel_no;
                hotel = hotel.Where(x => x.Holtel_no.StartsWith(Holtel_no));
            }
            if (!string.IsNullOrEmpty(Cname))
            {
                ViewBag.Cname = Cname;
                hotel = hotel.Where(x => x.Cname.Contains(Cname) || x.ShortName.Contains(Cname));
            }
            if (!string.IsNullOrEmpty(Ename))
            {
                ViewBag.Ename = Ename;
                hotel = hotel.Where(x => x.Ename.Contains(Ename));
            }
            if (Status != null)
            {
                ViewBag.Status = Status;
                hotel = hotel.Where(x => x.Status == Status);
            }
            if (CityIndex != null)
            {
                ViewBag.CityIndex = CityIndex;
                hotel = hotel.Where(x => x.CityIndex == CityIndex);
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
            ViewData["DataCount"] = hotel.Count();
            ViewBag.StatusNum = GetStuatus.DefualStatus();
            var result = hotel.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }

        // GET: Hotels/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Hotel hotel = db.Hotel.Find(id);
        //    if (hotel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.ShowStuatus = GetStuatus.ValidaStatus(hotel.Status);
        //    return View(hotel);
        //}

        // GET: Hotels/Create
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
            ViewBag.Star = sr.StarList;//顯示星級選項
            return View();
        }

        // POST: Hotels/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityDistrictIndex,CountryIndex,CityIndex,HotelIndex,ShortName,Ename,Cname,Area,Tele_CountryCode,Tele_Area,Tele1,tele2,Contact1,Contact1_Tele,Contact2,Contact2_tele,Fax,Email,URL,Blog,Longitude,Latitude,Addr,Introduction,Locate_des,Star,Checkin,Checkout,BuildingNo,TopNo,TotalRoom,Status,CreateBy,UpdateBy")] Hotel hotel)
        {
            string Company = Session["ComnpanyNo"].ToString();
            AutoCode AC = new AutoCode();
            string countryCode = db.Country01.Find(hotel.CountryIndex).Country_no;
            string cityCode = db.City03.Find(hotel.CityIndex).City_no;
            hotel.CreateBy = Convert.ToInt32(User.Identity.Name);
            hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
            hotel.CreateBy_Time = DateTime.Now;
            hotel.UpdateBy_Time = DateTime.Now;
            try 
            { 
                 if (ModelState.IsValid)
                    {
                        hotel.CompanyNo = Company;
                        hotel.Holtel_no = AC.GetAutoCodeH(countryCode, cityCode);
                        db.Hotel.Add(hotel);
                        db.SaveChanges();
                        return RedirectToAction("AddTypeTheme", new { id = hotel.HotelIndex });
                    }
            }
            catch
            {
                getViewData(hotel);
                return View(hotel);
            }
            getViewData(hotel);
            return View(hotel);
        }

        public ActionResult UploadImages(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var hotel = db.Hotel.Find(id);
            string Company = Session["ComnpanyNo"].ToString();
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            Sc_UploadImage su = new Sc_UploadImage();
            su.hotelIndex =hotel.HotelIndex;
            su.cname = hotel.Cname;
            su.holtel_no = hotel.Holtel_no;
            return View(su);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImages(IEnumerable<HttpPostedFileBase> imageUpload, string[] imgInfo, int hotelIndex, string holtel_no, string cname)
        { 
            string fileName = "";
            string compyNo = Session["ComnpanyNo"].ToString();
            string path = Server.MapPath("~/images/" + compyNo + "/Hotel");
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
                            ModelState.AddModelError("imageUpload", "只能上傳圖片");
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
                        img.HotelIndex = hotelIndex;
                        img.ImgInfo = imgInfo[i];
                        img.ImgPath = "/images/" + compyNo + "/Hotel/" + fileName;
                        img.CreateBy = Convert.ToInt32(User.Identity.Name);
                        img.CreateBy_Time = DateTime.Now;
                        img.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        img.UpdateBy_Time = DateTime.Now;
                        img.POI_Type = 3;
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
                    su.hotelIndex = hotelIndex;
                    su.cname = cname;
                    su.holtel_no = holtel_no;
                    ViewBag.success = "上傳成功";
                    return View(su);
                }
                catch
                {
                    su.hotelIndex = hotelIndex;
                    su.cname = cname;
                    su.holtel_no = holtel_no;
                    ViewBag.success = "上傳失敗";
                    return View(su);
                }
            }
            su.hotelIndex = hotelIndex;
            su.cname = cname;
            su.holtel_no = holtel_no;
            ViewBag.success = "上傳失敗";
            return View(su);
        }

        // GET: Hotels/Edit/5
        public ActionResult Edit(int? id)
        {
            var country = db.Country01.Where(x => x.Status == 1);//選出已完成國家
            var firstCountry = country.First();//選出第一個已完成的國家
            Star sr = new Star();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hotel = db.Hotel.Find(id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            sr.SetStar(hotel.Star);//設定星級
            ViewBag.Star = sr.StarList;//顯示星級
            getViewData(hotel);
            ViewBag.Status = GetStuatus.GetStatus(hotel.Status);//顯示目前狀態
            return View(hotel);
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

        // POST: Hotels/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityDistrictIndex,CompanyNo,Holtel_no,CountryIndex,CityIndex,HotelIndex,ShortName,Ename,Cname,Area,Tele_CountryCode,Tele_Area,Tele1,tele2,Contact1,Contact1_Tele,Contact2,Contact2_tele,Fax,Email,URL,Blog,Longitude,Latitude,Addr,Introduction,Locate_des,Star,Checkin,Checkout,BuildingNo,TopNo,TotalRoom,Status,CreateBy,CreateBy_Time,UpdateBy")] Hotel hotel)
        {
            Star sr = new Star();
            if (ModelState.IsValid)
            {
                hotel.UpdateBy_Time = DateTime.Now;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                db.Entry(hotel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            sr.SetStar(hotel.Star);//設定星級
            ViewBag.Star = sr.StarList;//顯示星級
            var hotel2 = db.Hotel.Find(hotel.HotelIndex);
            hotel.Country01 = hotel2.Country01;
            hotel.City03 = hotel2.City03;
            getViewData(hotel);
            ViewBag.Status = GetStuatus.GetStatus(hotel.Status);//顯示目前狀態
            return View(hotel);
        }
        public ActionResult AddRoomTypeOutLook(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var hotel = db.Hotel.Find(id);
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> RoomTypeIndex = new List<int>();
            List<int> OutLookIndex = new List<int>();
            if (hotel.RoomType.Count() > 0)
            {
                foreach (var item in hotel.RoomType)
                {
                    if (item.Main == true)
                    {
                        RoomTypeIndex.Add(item.RoomType_index);
                    }
                }
            }
            if (hotel.HotelOutLook.Count() > 0)
            {
                foreach (var item in hotel.HotelOutLook)
                {
                    if (item.Main == true)
                    {
                        OutLookIndex.Add(item.OutLook_index);
                    }
                }
            }
            RoomOutlook roomOutlook = new RoomOutlook(Company, hotel.HotelIndex, RoomTypeIndex.ToArray(), OutLookIndex.ToArray());

            return View(roomOutlook);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoomTypeOutLook(RoomOutlook roomOutlook)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatHotel1 = db.RoomType.Where(x => x.HotelIndex == roomOutlook.HotelIndex && x.CompanyNo == Company);//找出所有房型對應表已經有的資料
            var repeatHotel1 = db.HotelOutLook.Where(x => x.HotelIndex == roomOutlook.HotelIndex && x.CompanyNo == Company);//找出所有外觀對應表已經有的資料
            foreach (var item in RepeatHotel1)//將所有房型對應表已有資料改為false
            {
                var hotel = db.RoomType.Find(item.RoomTypeIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatHotel1)//將所有外觀對應表已有資料改為false
            {
                var hotel = db.HotelOutLook.Find(item.HotelOutLookIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (roomOutlook.OutLook_no != null)//判斷使用者是否有選擇任何外觀
            {
                foreach (int i in roomOutlook.OutLook_no)//取出所有選擇的外觀
                {
                    HotelOutLook ho = new HotelOutLook()
                    {
                        HotelIndex = roomOutlook.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = roomOutlook.CompanyNo,
                        OutLook_index = i
                    };
                    var RepeatHotel = db.HotelOutLook.Where(x => x.HotelIndex == i && x.HotelIndex == ho.HotelIndex && x.CompanyNo == Company);//找出所有此外觀的對應資料
                    if (RepeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = RepeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        roomOutlook.hotelOutLook.Add(ho);//沒有則加入此筆資料
                    }

                }
            }

            if (roomOutlook.RoomType_no != null)//判斷使用者是否有選擇任何房型
            {
                foreach (int i in roomOutlook.RoomType_no)//取出所有選擇的房型
                {
                    RoomType rt = new RoomType()
                    {
                        HotelIndex = roomOutlook.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = roomOutlook.CompanyNo,
                        RoomType_index = i
                    };
                    var repeatHotel = db.RoomType.Where(x => x.RoomType_index == i && x.HotelIndex == rt.HotelIndex && x.CompanyNo == Company);//找出所有此旅館設施的對應資料
                    if (repeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = repeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        roomOutlook.roomType.Add(rt);//沒有則加入此筆資料
                    }
                }
            }
            try
            {
                db.HotelOutLook.AddRange(roomOutlook.hotelOutLook);
                db.RoomType.AddRange(roomOutlook.roomType);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddRoomTypeOutLook", new { roomOutlook.HotelIndex });
            }
            TempData["Success"] = "更改成功";
            return RedirectToAction("AddRoomTypeOutLook", new {roomOutlook.HotelIndex });
        }
        public ActionResult AddNearbyHotelFaci(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var hotel = db.Hotel.Find(id);
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> NearbyIndex = new List<int>();
            List<int> HotelFaciIndex = new List<int>();
            if (hotel.NearbyFacility.Count() > 0)
            {
                foreach (var item in hotel.NearbyFacility)
                {
                    if (item.Main == true)
                    {
                        NearbyIndex.Add(item.NearbyFaci_no);
                    }
                }
            }
            if (hotel.HotelFacility.Count() > 0)
            {
                foreach (var item in hotel.HotelFacility)
                {
                    if (item.Main == true)
                    {
                        HotelFaciIndex.Add(item.HotelFaci_no);
                    }
                }
            }
            NearbyHotelFcai nearbyHotelFcai = new NearbyHotelFcai(Company, hotel.HotelIndex, NearbyIndex.ToArray(), HotelFaciIndex.ToArray());

            return View(nearbyHotelFcai);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNearbyHotelFaci(NearbyHotelFcai nearbyHotelFcai)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatHotel1 = db.NearbyFacility.Where(x => x.HotelIndex == nearbyHotelFcai.HotelIndex && x.CompanyNo == Company);//找出所有附近設施對應表已經有的資料
            var repeatHotel1 = db.HotelFacility.Where(x => x.HotelIndex == nearbyHotelFcai.HotelIndex && x.CompanyNo == Company);//找出所有旅館設備對應表已經有的資料
            foreach (var item in RepeatHotel1)//將所有附近設施對應表已有資料改為false
            {
                var hotel = db.NearbyFacility.Find(item.NearbyFacilityIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatHotel1)//將所有旅館設備對應表已有資料改為false
            {
                var hotel = db.HotelFacility.Find(item.HotelFacilityIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (nearbyHotelFcai.NearbyFaci_no != null)//判斷使用者是否有選擇任何附近設施
            {
                foreach (int i in nearbyHotelFcai.NearbyFaci_no)//取出所有選擇的附近設施
                {
                    NearbyFacility hf = new NearbyFacility()
                    {
                        HotelIndex = nearbyHotelFcai.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = nearbyHotelFcai.CompanyNo,
                        NearbyFaci_no = i
                    };
                    var RepeatHotel = db.NearbyFacility.Where(x => x.HotelIndex == i && x.HotelIndex == hf.HotelIndex && x.CompanyNo == Company);//找出所有此附近設施的對應資料
                    if (RepeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = RepeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        nearbyHotelFcai.Nearby.Add(hf);//沒有則加入此筆資料
                    }

                }
            }

            if (nearbyHotelFcai.HotelFaci_no != null)//判斷使用者是否有選擇任何旅館設施
            {
                foreach (int i in nearbyHotelFcai.HotelFaci_no)//取出所有選擇的旅館設施
                {
                    HotelFacility hf = new HotelFacility()
                    {
                        HotelIndex = nearbyHotelFcai.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = nearbyHotelFcai.CompanyNo,
                        HotelFaci_no = i
                    };
                    var repeatHotel = db.HotelFacility.Where(x => x.HotelFaci_no == i && x.HotelIndex == hf.HotelIndex && x.CompanyNo == Company);//找出所有此旅館設施的對應資料
                    if (repeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = repeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        nearbyHotelFcai.HotelFaci.Add(hf);//沒有則加入此筆資料
                    }
                }
            }
            try
            {
                 db.NearbyFacility.AddRange(nearbyHotelFcai.Nearby);
                 db.HotelFacility.AddRange(nearbyHotelFcai.HotelFaci);
                 db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddNearbyHotelFaci", new { id = nearbyHotelFcai.HotelIndex });
            }      
            TempData["Success"] = "更改成功";
            return RedirectToAction("AddNearbyHotelFaci", new {id = nearbyHotelFcai.HotelIndex });
        }

        public ActionResult AddSerFaci(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var hotel = db.Hotel.Find(id);
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> ServiceIndex = new List<int>();
            List<int> RoomFaciIndex = new List<int>();
            if (hotel.HotelService.Count() > 0)
            {
                foreach (var item in hotel.HotelService)
                {
                    if (item.Main == true)
                    {
                        ServiceIndex.Add(item.Hotel_Ser_no);
                    }
                }
            }
            if (hotel.RoomFacility.Count() > 0)
            {
                foreach (var item in hotel.RoomFacility)
                {
                    if (item.Main == true)
                    {
                        RoomFaciIndex.Add(item.RoomFaci_no);
                    }
                }
            }
            ServiceRoomFaci serviceRoomFaci = new ServiceRoomFaci(Company, hotel.HotelIndex, ServiceIndex.ToArray(), RoomFaciIndex.ToArray());

            return View(serviceRoomFaci);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSerFaci(ServiceRoomFaci serviceRoomFaci)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatHotel1 = db.HotelService.Where(x => x.HotelIndex == serviceRoomFaci.HotelIndex && x.CompanyNo == Company);//找出所有服務對應表已經有的資料
            var repeatHotel1 = db.RoomFacility.Where(x => x.HotelIndex == serviceRoomFaci.HotelIndex && x.CompanyNo == Company);//找出所有房內設備對應表已經有的資料
            foreach (var item in RepeatHotel1)//將所有服務對應表已有資料改為false
            {
                var hotel = db.HotelService.Find(item.HotelServiceIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatHotel1)//將所有房內對應表已有資料改為false
            {
                var hotel = db.RoomFacility.Find(item.RoomFacIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (serviceRoomFaci.Service_no != null)//判斷使用者是否有選擇任何服務
            {
                foreach (int i in serviceRoomFaci.Service_no)//取出所有選擇的服務
                {
                    HotelService hs = new HotelService()
                    {
                        HotelIndex = serviceRoomFaci.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = serviceRoomFaci.CompanyNo,
                        Hotel_Ser_no = i
                    };
                    var RepeatHotel = db.HotelService.Where(x => x.HotelIndex == i && x.HotelIndex == hs.HotelIndex && x.CompanyNo == Company);//找出所有此服務的對應資料
                    if (RepeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = RepeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        serviceRoomFaci.Service.Add(hs);//沒有則加入此筆資料
                    }

                }
            }

            if (serviceRoomFaci.Facility_no != null)//判斷使用者是否有選擇任何房內設備
            {
                foreach (int i in serviceRoomFaci.Facility_no)//取出所有選擇的房內設備
                {
                    RoomFacility rf = new RoomFacility()
                    {
                        HotelIndex = serviceRoomFaci.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = serviceRoomFaci.CompanyNo,
                        RoomFaci_no = i
                    };
                    var repeatHotel = db.RoomFacility.Where(x => x.RoomFaci_no == i && x.HotelIndex == rf.HotelIndex && x.CompanyNo == Company);//找出所有此型態的對應資料
                    if (repeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = repeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        serviceRoomFaci.Facility.Add(rf);//沒有則加入此筆資料
                    }
                }
            }
            try
            {
                db.HotelService.AddRange(serviceRoomFaci.Service);
                db.RoomFacility.AddRange(serviceRoomFaci.Facility);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddSerFaci", new { id = serviceRoomFaci.HotelIndex });
            }        
            TempData["Success"] = "更改成功";
            return RedirectToAction("AddSerFaci", new {id=serviceRoomFaci.HotelIndex });
        }
        public ActionResult AddTypeTheme(int? id)
        {
            string Company = Session["ComnpanyNo"].ToString();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var hotel = db.Hotel.Find(id);
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            List<int> TypeIndex = new List<int>();
            List<int> ThemeIndex = new List<int>();
            if (hotel.Hotel_Theme.Count() > 0)
            {
                foreach (var item in hotel.Hotel_Theme)
                {
                    if (item.Main == true)
                    {
                        ThemeIndex.Add(item.Theme_Index);
                    }
                }
            }
            if (hotel.Hotel_Type.Count() > 0)
            {
                foreach (var item in hotel.Hotel_Type)
                {
                    if (item.Main == true)
                    {
                        TypeIndex.Add(item.Type_Index);
                    }
                }
            }
            TypeThemeHViewModel typeTheme = new TypeThemeHViewModel(Company, hotel.HotelIndex, TypeIndex.ToArray(), ThemeIndex.ToArray());

            return View(typeTheme);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddTypeTheme(TypeThemeHViewModel typeTheme)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var RepeatHotel1 = db.Hotel_Theme.Where(x => x.HotelIndex == typeTheme.HotelIndex && x.CompanyNo == Company);//找出所有主題對應表已經有的資料
            var repeatHotel1 = db.Hotel_Type.Where(x => x.HotelIndex == typeTheme.HotelIndex && x.CompanyNo == Company);//找出所有型態對應表已經有的資料
            foreach (var item in RepeatHotel1)//將所有主題對應表已有資料改為false
            {
                var hotel = db.Hotel_Theme.Find(item.HotelThemeIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            foreach (var item in repeatHotel1)//將所有型態對應表已有資料改為false
            {
                var hotel = db.Hotel_Type.Find(item.HotelTypeIndex);
                hotel.Main = false;
                hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                hotel.UpdateBy_Time = DateTime.Now;
                db.Entry(hotel).State = EntityState.Modified;
                //db.SaveChanges();
            }
            if (typeTheme.Theme_index != null)//判斷使用者是否有選擇任何主題
            {
                foreach (int i in typeTheme.Theme_index)//取出所有選擇的主題
                {
                    Hotel_Theme ht = new Hotel_Theme()
                    {
                        HotelIndex = typeTheme.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Theme_Index = i
                    };
                    var RepeatHotel = db.Hotel_Theme.Where(x => x.Theme_Index == i && x.HotelIndex == ht.HotelIndex && x.CompanyNo == Company);//找出所有此主題的對應資料
                    if (RepeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = RepeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.theme1.Add(ht);//沒有則加入此筆資料
                    }

                }
            }

            if (typeTheme.Type_index != null)//判斷使用者是否有選擇任何型態
            {
                foreach (int i in typeTheme.Type_index)//取出所有選擇的型態
                {
                    Hotel_Type ht = new Hotel_Type()
                    {
                        HotelIndex = typeTheme.HotelIndex,
                        CreateBy = Convert.ToInt32(User.Identity.Name),
                        CreateBy_Time = DateTime.Now,
                        UpdateBy = Convert.ToInt32(User.Identity.Name),
                        UpdateBy_Time = DateTime.Now,
                        Main = true,
                        CompanyNo = typeTheme.CompanyNo,
                        Type_Index = i
                    };
                    var repeatHotel = db.Hotel_Type.Where(x => x.Type_Index == i && x.HotelIndex == ht.HotelIndex && x.CompanyNo == Company);//找出所有此型態的對應資料
                    if (repeatHotel.Count() > 0)//判斷是否有對應資料有則改成True
                    {
                        var hotel = repeatHotel.FirstOrDefault();
                        hotel.Main = true;
                        hotel.UpdateBy = Convert.ToInt32(User.Identity.Name);
                        hotel.UpdateBy_Time = DateTime.Now;
                        db.Entry(hotel).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        typeTheme.type1.Add(ht);//沒有則加入此筆資料
                    }
                }
            }

            try
            {
                db.Hotel_Theme.AddRange(typeTheme.theme1);
                db.Hotel_Type.AddRange(typeTheme.type1);
                db.SaveChanges();
            }
            catch
            {
                TempData["Success"] = "更改失敗";
                return RedirectToAction("AddTypeTheme", new { id = typeTheme.HotelIndex });
            }
            TempData["Success"] = "更改成功";
            
            return RedirectToAction("AddTypeTheme", new {id = typeTheme.HotelIndex });
        }

        public ActionResult SelectOption(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("index");
            }
            var hotel = db.Hotel.Find(id);
            return View(hotel);
        }

        public void getViewData(Hotel hotel)
        {
            string Company = Session["ComnpanyNo"].ToString();
            Star sr = new Star();
            ViewBag.CreateBy = db.HRInfo.Find(hotel.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(hotel.UpdateBy).EmpName;
            var country = db.Country01.Where(x => x.Status == 1 && x.CompanyNo == Company).Select(x => new { x.CountryIndex, Cname = x.Country_no + " " + x.Cname });
            ViewBag.CountryIndex = new SelectList(country, "CountryIndex", "Cname", hotel.CountryIndex);
            var selectCity = db.City03.Where(x => x.CountryIndex == hotel.CountryIndex && x.Status == 1 && x.CompanyNo == Company)
                .Select(x => new { x.CityIndex, Cname = x.City_no + " " + x.Cname });
            ViewBag.CityIndex = new SelectList(selectCity, "CityIndex", "Cname");
            if (selectCity != null)
            {
                var cityDistrict = db.CityDistrict.Where(x => x.Status == 1 && x.CityIndex == hotel.CityIndex && x.CompanyNo == Company)
                    .Select(x => new { x.CityDistrictIndex, Cname = x.CityDistrictCode + " " + x.DisCname });
                ViewBag.CityDistrictIndex = new SelectList(cityDistrict, "CityDistrictIndex", "Cname", hotel.CityDistrictIndex);
            }
            else
            {
                ViewBag.CityDistrictIndex = new SelectList("");
            }
            ViewBag.Status = GetStuatus.GetStatus(hotel.Status);//顯示完成選項
            sr.SetStar(hotel.Star);//設定星級
            ViewBag.Star = sr.StarList;//顯示星級選項
        }

        // GET: Hotels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotel.Find(id);
            string Company = Session["ComnpanyNo"].ToString();
            if (hotel.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        // POST: Hotels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Hotel hotel = db.Hotel.Find(id);
            hotel.Status = 3;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetCity_no(int CountryIndex)
        {
            string Company = Session["ComnpanyNo"].ToString();
            var NowCity = db.City03.Where(x => x.CountryIndex == CountryIndex && x.Status == 1 && x.CompanyNo == Company);
            var NowCity2 = NowCity.Select(x => new { x.CityIndex, x.City_no, x.Cname });
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
