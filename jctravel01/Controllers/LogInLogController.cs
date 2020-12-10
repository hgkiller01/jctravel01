using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using jctravel01.Models;
using jctravel01.Models.ViewModel;

namespace jctravel01.Controllers
{
    public class LogInLogController : Controller
    {
        TravelContainer db = new TravelContainer();
        private int pagesize = 5;
        // GET: LogInLog
        public ActionResult Index(int? Select, string Search, int page = 1)
        {
            var LoginLog = db.logInLog.OrderByDescending(x => x.LogInTime).Where(x => x.CompanyNo == "00001");
            int CurrentPage = page < 1 ? 1 : page; //若目前分頁小於1則目前分頁設為1
            if (Select != null)
            {
                if (Select == 1)
                {
                    LoginLog = LoginLog.Where(x => x.HRInfo.EmpNo.StartsWith(Search));
                }
                else if(Select ==2)
                {
                    LoginLog = LoginLog.Where(x => x.HRInfo.EmpName.Contains(Search));
                }
                else if (Select == 3)
                {
                    LoginLog = LoginLog.Where(x => x.LogInPlace.Contains(Search));
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Search))
                {
                    LoginLog = LoginLog.Where(x => x.HRInfo.EmpNo.StartsWith(Search) || x.HRInfo.EmpName.Contains(Search) || x.LogInPlace.Contains(Search));
                }
            }
            ViewBag.Select = Select;
            ViewBag.Search = Search;
            Dictionary<int, string> searchList = new Dictionary<int, string>();
            searchList.Add(1, "員工代號");
            searchList.Add(2, "員工姓名");
            searchList.Add(3, "登入地點");
            ViewBag.SelectBar = new SelectList(searchList, "key", "value");
            ViewData["DataCount"] = LoginLog.Count();
            ViewBag.RowCountMin = CurrentPage * pagesize - 4;
            var result = LoginLog.ToPagedList(CurrentPage, pagesize);
            return View(result);
        }
    }
}