using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CaptchaMvc.Controllers;
using jctravel01.Models;
using jctravel01.Models.ViewModel;
using CaptchaMvc.HtmlHelpers;
using System.Web.Security;
using System.Data.Entity;
using System.Net;
using System.Net.Sockets;

namespace jctravel01.Controllers
{
    public class MemberLoginController : Controller
    {
        TravelContainer db = new TravelContainer();
        // GET: MemberLogin
        public ActionResult Index()
        {
            return View();
           
        }
        public string getIP()
        {
            string VisitorsIPAddr = string.Empty;
            //if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            //{
            //    VisitorsIPAddr = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //}
            if (Request.UserHostAddress.Length != 0)
            {
                VisitorsIPAddr = Request.UserHostAddress;
            }
            return VisitorsIPAddr;
            //string localIP = "?";
            //IPHostEntry host;
            //host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily == AddressFamily.InterNetwork)
            //    {
            //        localIP = ip.ToString();
            //    }
            //}
            //return localIP;
        }
        // GET: MemberLogin/Create

        // POST: MemberLogin/Create
        [HttpPost]
        [ActionName("Index")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberLogin memberLogin)
        {
            //取得使用者IP
            string VisitorsIPAddr = getIP();
            //判斷驗証碼
            if (this.IsCaptchaValid("驗證碼錯誤"))      
            {
                if (ModelState.IsValid)
                {
                    //找出員工資料
                    var member = db.HRInfo.Where(x => x.EmpNo == memberLogin.EmpNo && x.CompanyNo == memberLogin.CompanyNo).FirstOrDefault();
                    //找出目前IP匿名登入
                    var hackLogin = db.logInLog.Where(x => x.CompanyNo == "00000" && x.EmpIndex == 0 && x.LoginIp == VisitorsIPAddr && x.ErrorLog);
                    //匿名登入超過四次判斷
                    #region 亂登入四次以上
                    if (hackLogin.Count() > 3)
                    {
                        hackLogin = hackLogin.OrderByDescending(x => x.LogInTime);
                        var LastLgoin = hackLogin.FirstOrDefault();
                        //判斷是否一小時候登入
                        if (LastLgoin.LogInTime.AddMinutes(60) > DateTime.Now)
                        {
                            TempData["FailureHR"] = "請在一小時後重新登入";
                            return View(memberLogin);
                        }
                        else
                        {
                            foreach (var item in hackLogin)
                            {
                                item.ErrorLog = false;
                                db.Entry(item).State = EntityState.Modified;
                            }
                            db.SaveChanges();
                        }
                    }
                    #endregion
                    
                    PasMng pas = new PasMng();
                    #region //判斷是否有此人員
                    if (member == null)
                    {
                        LoginFailure(member,memberLogin.PassWord,VisitorsIPAddr);
                        TempData["FailureHR"] = "並無此帳號";
                        return View();
                    }
                    else
                    {
                        //找出此人員登入失敗次數
                        var failureLogin = db.logInLog.Where(x => x.CompanyNo == memberLogin.CompanyNo && x.EmpIndex == member.EmpIndex && x.ErrorLog);
                        int logincount = failureLogin.Count();
                        //若次數等於3次則鎖住帳號
                        if (failureLogin.Count() > 2)
                        {
                           member = db.HRInfo.Find(member.EmpIndex);
                           member.AllowLogIn = false;
                           db.Entry(member).State = EntityState.Modified;
                           db.SaveChanges();
                           LoginFailure(member, memberLogin.PassWord, VisitorsIPAddr);
                           TempData["LoginSuccess"] = "您的帳號已經鎖定，請相關人員解鎖";
                           return RedirectToAction("Index", "Home");
                        }
                        #region //人員是否能登入
                        if (member.AllowLogIn)
                        {
                            pas = db.PasMng.Where(x => x.EmpIndex == member.EmpIndex && x.newpsd == memberLogin.PassWord).FirstOrDefault();
                            if (pas != null)
                            {
                                //判斷離職時間
                                if (member.ResignDate != null)
                                {
                                    if (DateTime.Now > member.ResignDate)
                                    {
                                        member.AllowLogIn = false;
                                        db.Entry(member).State = EntityState.Modified;
                                        db.SaveChanges();
                                        TempData["LoginSuccess"] = "您的帳號已經無法使用";
                                        return RedirectToAction("Index", "Home");
                                    }
                                }
                                if (member.OnBoardDate > DateTime.Now)
                                {
                                    TempData["LoginSuccess"] = "您的帳號目前還無法使用";
                                    return RedirectToAction("Index", "Home");
                                }
                                #region 是否有固定IP
                                if (!string.IsNullOrEmpty(pas.CoIndex.CoIP))
                                {
                                    #region 驗證IP位置
                                    if (VisitorsIPAddr == pas.CoIndex.CoIP)
                                    {
                                        //若為新帳號則更改密碼
                                        if (pas.newpsd == pas.oldpsd)
                                        {
                                            Session["ChangePassword"] = true;
                                            return RedirectToAction("CPassWord", new { id = pas.EmpIndex });
                                        }
                                        LoginSuccess(member, VisitorsIPAddr, memberLogin.PassWord);
                                        TempData["LoginSuccess"] = "登入成功";
                                        //登入成功後 解除所有失敗的登入狀態
                                        var log2 = db.logInLog.Where(x => x.EmpIndex == member.EmpIndex);
                                        foreach (var item in log2)
                                        {
                                            item.ErrorLog = false;
                                            db.Entry(item).State = EntityState.Modified;
                                        }
                                        db.SaveChanges();
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return RedirectToAction("SecondLogin", new { id = pas.EmpIndex });
                                    }
                                    #endregion
                                }
                                else
                                {
                                    //若為新帳號則更改密碼
                                    if (pas.newpsd == pas.oldpsd)
                                    {
                                        Session["ChangePassword"] = true;
                                        return RedirectToAction("CPassWord", new { id = pas.EmpIndex });
                                    }
                                    LoginSuccess(member, VisitorsIPAddr, memberLogin.PassWord);
                                    TempData["LoginSuccess"] = "登入成功";
                                    //登入成功後 解除所有失敗的登入狀態
                                   var log1 = db.logInLog.Where(x => x.EmpIndex == member.EmpIndex);
                                   foreach (var item in log1)
                                    {
                                        item.ErrorLog = false;
                                        db.Entry(item).State = EntityState.Modified;
                                    }
                                    db.SaveChanges();
                                    return RedirectToAction("Index", "Home");
                                }
                                #endregion
                            }
                            else
                            {
                                LoginFailure(member, memberLogin.PassWord, VisitorsIPAddr);
                                TempData["FailureHR"] = "帳號密碼不符!請重新登入!";
                            }
                        }
                        else
                        {
                            LoginFailure(member, memberLogin.PassWord, VisitorsIPAddr);
                            TempData["FailureHR"] = "你的帳號已經鎖定";
                        }
                        #endregion
                    }
                    #endregion
                }
            }
            return View(memberLogin);
        }
        public void LoginSuccess(HRInfo hrInfo,string ip,string loginPass, string place ="" )
        {
            #region//成功登入後得到權限
            List<string> roles = new List<string>();
            foreach (var item in hrInfo.PermiMng)
            {
                
                if (item.Main)
                {
                    foreach (var item2 in item.PermiGruop.Authorze_index)
                    {
                        string permitText = item2.PermiIndex1.PermiNo;
                        Session[permitText] = true;
                        string MainPername = item2.PermiIndex1.MainPerName;
                        Session[MainPername] = true;
                        string read = "";
                        string change = "";
                        string share = "";
                        if (item2.ReadPermi)
                        {
                            read = item2.PermiIndex1.PermiNo + "R";
                            Session[read] = true;
                        }
                        if (item2.ChPermi)
                        {
                            change = item2.PermiIndex1.PermiNo + "C";
                            Session[change] = true;
                        }
                        if (item2.SharePermi)
                        {
                            share = item2.PermiIndex1.PermiNo + "S";
                            Session[share] = true;
                        }
                    }
                }
            }
            string userRole ="User";
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                1,
                hrInfo.EmpIndex.ToString(),
                DateTime.Now,
                DateTime.Now.AddMinutes(30),
                false,
                userRole,
                FormsAuthentication.FormsCookiePath
                );
            string encTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
            Session["ComnpanyNo"] = hrInfo.CompanyNo;
            Session["UserID"] = hrInfo.EmpIndex;
            Session["UserName"] = hrInfo.EmpName;
            #endregion
            
            #region //插入成功登入的記錄
            logInLog log = new logInLog();
            log.LogInIndex = Guid.NewGuid();
            log.EmpIndex = hrInfo.EmpIndex;
            log.CompanyNo = hrInfo.CompanyNo;
            log.LogInTime = DateTime.Now;
            if (!string.IsNullOrEmpty(place))
            {
                log.LogInPlace = place;
            }
            else
            {
                log.LogInPlace = "公司";                
            }
            log.LoginIp = ip;
            log.LogInPsd = loginPass;
            log.ErrorLog = false;
            log.ErrorLogInLog = false;
            log.AllowLogIn = true;
            db.logInLog.Add(log);
            db.SaveChanges();
            #endregion
        }
        public void LoginFailure(HRInfo hrInfo,string loginPass,string ip)
        {
            logInLog log = new logInLog();
           
            if (hrInfo == null)
            {
                //亂登入
                log.EmpIndex = 0;
                log.CompanyNo = "00000";
                log.LogInPlace = "失敗的登入位置";
            }
            else
            {
                log.LogInPlace = "失敗的登入位置";
                log.EmpIndex = hrInfo.EmpIndex;
                log.CompanyNo = hrInfo.CompanyNo;
            }
            log.LogInPsd = loginPass;
            log.LogInIndex = Guid.NewGuid();
            log.LogInTime = DateTime.Now;
            log.ErrorLog = true;
            log.LoginIp = ip;
            log.ErrorLogInLog = true;
            log.AllowLogIn = false;
            
            db.logInLog.Add(log);
            db.SaveChanges();
        }
        public ActionResult CPassWord(int? id,string place="")
        {
            ChangePassWord cp = new ChangePassWord();
            if (!string.IsNullOrEmpty(place))
            {
                cp.Place = place;
            }

            if (User.Identity.IsAuthenticated)
            {
                if (id == null)
                {
                    int Userid = int.Parse(Session["UserID"].ToString());
                    cp.empIndex = Userid;
                    return View(cp);
                }
                cp.empIndex = Convert.ToInt32(id);
                return View(cp);
            }
            if (Session["ChangePassword"] != null)
            {
                cp.empIndex = Convert.ToInt32(id);
                return View(cp);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("CPassWord")]
        public ActionResult CPassWordCfim(ChangePassWord pas)
        {
            PasMng pasMng = db.PasMng.Where(x => x.EmpIndex == pas.empIndex).FirstOrDefault();
            if (pasMng.oldpsd == pas.PassWord || pasMng.newpsd == pas.PassWord)
            {
                ModelState.AddModelError("PassWord", "不能與原本或上次密碼相同");
            }
            if (ModelState.IsValid)
            {
                pasMng.UpdateBy = pas.empIndex;
                pasMng.UpdateBy_Time = DateTime.Now;
                pasMng.oldpsd = pasMng.newpsd;
                pasMng.newpsd = pas.PassWord;
                var member = db.HRInfo.Find(pasMng.EmpIndex);
                if (User.Identity.IsAuthenticated)
                {
                    db.Entry(pasMng).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["LoginSuccess"] = "修改成功";
                    return RedirectToAction("Index", "Home");
                }
                db.Entry(pasMng).State = EntityState.Modified;
                LoginSuccess(member,getIP(),pas.PassWord,pas.Place);
                Session.Remove("ChangePassword");
                TempData["LoginSuccess"] = "登入成功";
                return RedirectToAction("Index", "Home");
            }
            return View(pas);
        }
        //[ChildActionOnly]
        public ActionResult SecondLogin(int? id)
        {
            MemberLogin2 member = new MemberLogin2();
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            member.empIndex = Convert.ToInt32(id);
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("SecondLogin")]
        public ActionResult PlaceLogin(MemberLogin2 member)
        {
            if (member == null)
            {
                return HttpNotFound();
            }
            var nowLogin = db.HRInfo.Find(member.empIndex);
            var pas = db.PasMng.Where(x => x.EmpIndex == nowLogin.EmpIndex).FirstOrDefault();
            if (nowLogin.EmpID == member.empID && nowLogin.EmpBday == member.BirthDay)
            {
                if (pas.newpsd == pas.oldpsd)
                {
                    Session["ChangePassword"] = true;
                    return RedirectToAction("CPassWord", new { id = pas.EmpIndex });
                }
                if (!string.IsNullOrEmpty(member.AtWhere))
                {
                    LoginSuccess(nowLogin, getIP(), pas.newpsd, member.AtWhere);
                }
                else
                {
                    LoginSuccess(nowLogin, getIP(), pas.newpsd, member.Location);
                }
                
            }
            TempData["LoginSuccess"] = "登入成功";
            return RedirectToAction("Index","Home");
        }
      
    }
}
