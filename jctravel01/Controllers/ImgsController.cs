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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace jctravel01.Controllers
{
    [Authorize]
    [AuthorizeCompny]
    public class ImgsController : Controller
    {
        private TravelContainer db = new TravelContainer();
        private int pagesize = 10;
        // GET: Imgs
        public ActionResult Index(int? Poi_Type, int? imgSearch, string Search, int page = 1)
        {
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            string Company = Session["ComnpanyNo"].ToString();
            imageViewModel ivm = new imageViewModel(Company);
            IEnumerable<Img> now = ivm.images;
            //var Nowimg = db.Img.OrderBy(x => x.ImgIndex).Where(x => x.CompanyNo == "00001");
            //var nowimg = db.Hotel.AsEnumerable();
         
            if(Poi_Type != null)
            {
                now = now.Where(x => x.POI_Type == Poi_Type);
            }
            if (!string.IsNullOrEmpty(Search))
            {
                switch (imgSearch)
                {
                    case 1:
                        if (Poi_Type == 1)
                        {
                            now = now.Where(x => x.Scenery.Cname.Contains(Search) || x.Scenery.ShortName.Contains(Search));
                        }
                        else if (Poi_Type == 2)
                        {
                            now = now.Where(x => x.Restaurant.Cname.Contains(Search) || x.Restaurant.ShortName.Contains(Search));
                        }
                        else if (Poi_Type == 3)
                        {
                            now = now.Where(x => x.Hotel.Cname.Contains(Search) || x.Restaurant.ShortName.Contains(Search));
                        }
                        else 
                        {
                            now = now.Where(x => x.Scenery.Cname.Contains(Search) ||
                                  x.Hotel.Cname.Contains(Search) ||
                                  x.Restaurant.Cname.Contains(Search));
                        }
                        break;
                    case 2:
                        if (Poi_Type == 1)
                        {
                            now = now.Where(x => x.Scenery.Ename.Contains(Search));
                        }
                        else if (Poi_Type == 2)
                        {
                            now = now.Where(x => x.Restaurant.Ename.Contains(Search));
                        }
                        else if (Poi_Type == 3)
                        {
                            now = now.Where(x => x.Hotel.Ename.Contains(Search));
                        }
                        else
                        {
                            now = now.Where(x => x.Scenery.Ename.Contains(Search) ||
                                  x.Hotel.Ename.Contains(Search) ||
                                  x.Restaurant.Ename.Contains(Search));
                        }
                        break;
                    case 3:
                        if (Poi_Type == 1)
                        {
                            now = now.Where(x => x.Scenery.Scenery_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 2)
                        {
                            now = now.Where(x => x.Restaurant.Rest_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 3)
                        {
                            now = now.Where(x => x.Hotel.Holtel_no.StartsWith(Search));
                        }
                        else
                        {
                            now = now.Where(x => x.Scenery.Scenery_no.Contains(Search) ||
                                  x.Hotel.Holtel_no.Contains(Search) ||
                                  x.Restaurant.Rest_no.Contains(Search));
                        }
                        break;
                    case 4:
                        if (Poi_Type == 1)
                        {
                            now = now.Where(x => x.Scenery.City03.City_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 2)
                        {
                            now = now.Where(x => x.Restaurant.City03.City_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 3)
                        {
                            now = now.Where(x => x.Hotel.City03.City_no.StartsWith(Search));
                        }
                         else 
                        {
                            now = now.Where(x => x.Scenery.City03.City_no.StartsWith(Search) ||
                                  x.Hotel.City03.City_no.StartsWith(Search) ||
                                  x.Restaurant.City03.City_no.StartsWith(Search));
                        }
                        break;
                    default:
                        if (Poi_Type == 1)
                        {
                            now = now.Where(x => x.Scenery.Cname.Contains(Search) || 
                                x.Scenery.ShortName.Contains(Search) ||
                                x.Scenery.Ename.Contains(Search) || x.Scenery.Scenery_no.StartsWith(Search)
                                || x.Scenery.City03.City_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 2)
                        {
                            now = now.Where(x => x.Hotel.Cname.Contains(Search) ||
                                x.Hotel.ShortName.Contains(Search) ||
                                x.Hotel.Ename.Contains(Search) || x.Hotel.Holtel_no.StartsWith(Search)
                                || x.Hotel.City03.City_no.StartsWith(Search));
                        }
                        else if (Poi_Type == 3)
                        {
                            now = now.Where(x => x.Restaurant.Cname.Contains(Search) ||
                                x.Restaurant.ShortName.Contains(Search) ||
                                x.Restaurant.Ename.Contains(Search) || x.Restaurant.Rest_no.StartsWith(Search)
                                || x.Restaurant.City03.City_no.StartsWith(Search));
                        }
                        break;
                }
            }           
            ViewBag.Search = Search;
            ViewBag.Poi_Type = Poi_Type;
            ViewBag.imgSearch = imgSearch;
            ViewData["DataCount"] = now.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            
            var poiList = new GetPoi();
            var ImgSearch = new GetImgSearch();
            ViewBag.POI_TypeNum = new SelectList(poiList.poi, "poiIndex", "PoiName");
            ViewBag.imgSearchNum = new SelectList(ImgSearch.search, "searchType", "searchText");
            var result = now.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }


        // GET: Imgs/Details/5
        //public ActionResult Details(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Img img = db.Img.Find(id);
        //    if (img == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(img);
        //}

        // GET: Imgs/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Imgs/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ImgIndex,CompanyNo,POI_Type,Scenery_index,RestIndex,HotelIndex,OrderNo,ImgPath,ImgInfo,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] Img img)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Img.Add(img);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(img);
        //}

        // GET: Imgs/Edit/5
        public ActionResult PictureList(int? id, int? poi_type)
        {
            PictureListViewModel picture = new PictureListViewModel();
            if (id == null || poi_type == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (poi_type == 1)
            {
                picture.scenery = db.Scenery.Find(id);
                picture.Company = picture.scenery.CompanyNo;
            }
            else if (poi_type == 2)
            {
                picture.restaurant = db.Restaurant.Find(id);
                picture.Company = picture.restaurant.CompanyNo;
            }
            else if (poi_type == 3)
            {
                picture.hotel = db.Hotel.Find(id);
                picture.Company = picture.hotel.CompanyNo;
            }
            else
            {
                return HttpNotFound();
            }
            string Company = Session["ComnpanyNo"].ToString();
            if (picture.Company != Company)
            {
                return HttpNotFound();
            }
            return View(picture);
        }
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Img img = db.Img.Find(id);
            string Company = Session["ComnpanyNo"].ToString();
            if (img.CompanyNo != Company)
            {
                return HttpNotFound();
            }
            if (img == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = GetStuatus.GetStatus(img.Status);
                if (img.POI_Type == 1)
                {
                    ViewBag.PoiName = img.Scenery.Cname;
                }
                else if (img.POI_Type == 2)
                {
                    ViewBag.PoiName = img.Restaurant.Cname;
                }
                else
                {
                    ViewBag.PoiName = img.Hotel.Cname;
                }
                ViewBag.CreateBy = db.HRInfo.Find(img.CreateBy).EmpName;
                ViewBag.UpdateBy = db.HRInfo.Find(img.UpdateBy).EmpName;
            return View(img);
        }
        public ActionResult Action()
        {
            return View();
        }

        // POST: Imgs/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ImgIndex,Status,CompanyNo,POI_Type,Scenery_index,RestIndex,HotelIndex,OrderNo,ImgPath,ImgInfo,CreateBy,CreateBy_Time,UpdateBy,UpdateBy_Time")] Img img, HttpPostedFileBase imageUpload)
        {
                if (imageUpload != null)
                {
                    if (imageUpload.ContentLength > 0)
                    {
                        string fileExten = imageUpload.ContentType;
                        if (fileExten != "image/jpg" && fileExten != "image/jpeg" && fileExten != "image/png")
                        {
                            ModelState.AddModelError("imageUpload", "只能上傳JPG與PNG");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("imageUpload", "檔案大小不正確");
                    }
                }

            if (ModelState.IsValid)
            {
                if (imageUpload != null) 
                {
                    System.IO.File.Delete(Server.MapPath("~" + img.ImgPath));
                    Random rm = new Random();
                    string path = "";
                    string fileExton = Path.GetExtension(imageUpload.FileName);
                    string fileName = DateTime.Now.ToString("yyyyMMddhhmmss") + rm.Next(9) + fileExton;
                    if (img.POI_Type == 1)
                    {
                        path = Server.MapPath("~/images/" + img.CompanyNo + "/Scenery");
                        img.ImgPath = "/images/" + img.CompanyNo + "/Scenery/" + fileName;
                    }
                    if (img.POI_Type == 2)
                    {
                        path = Server.MapPath("~/images/" + img.CompanyNo + "/Restaurant");
                        img.ImgPath = "/images/" + img.CompanyNo + "/Restaurant/" + fileName;
                    }
                    if (img.POI_Type == 3)
                    {
                        path = Server.MapPath("~/images/" + img.CompanyNo + "/Hotel");
                        img.ImgPath = "/images/" + img.CompanyNo + "/Hotel/" + fileName;
                    }
                    string path2 = Path.Combine(path, fileName);
                    imageUpload.SaveAs(path2);
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
                }
                img.UpdateBy = Convert.ToInt32(User.Identity.Name);
                img.UpdateBy_Time = DateTime.Now;
                db.Entry(img).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (img.POI_Type == 1)
            {
                ViewBag.PoiName = img.Scenery.Cname;
            }
            else if (img.POI_Type == 2)
            {
                ViewBag.PoiName = img.Restaurant.Cname;
            }
            else
            {
                ViewBag.PoiName = img.Hotel.Cname;
            }
            ViewBag.CreateBy = db.HRInfo.Find(img.CreateBy).EmpName;
            ViewBag.UpdateBy = db.HRInfo.Find(img.UpdateBy).EmpName;
            ViewBag.Status = GetStuatus.GetStatus(img.Status);
            return View(img);
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


        // GET: Imgs/Delete/5
        //public ActionResult Delete(long? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Img img = db.Img.Find(id);
        //    if (img == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(img);
        //}

        // POST: Imgs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(long id)
        //{
        //    Img img = db.Img.Find(id);
        //    db.Img.Remove(img);
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
