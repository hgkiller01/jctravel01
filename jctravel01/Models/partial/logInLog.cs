using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(logInLogMD))]
    public partial class logInLog
    {
        public class logInLogMD
        {
            public System.Guid LogInIndex { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("員工編號")]
            public int EmpIndex { get; set; }
            [DisplayName("登入位置")]
            public string LogInPlace { get; set; }
            [DisplayName("登入時間")]
            public System.DateTime LogInTime { get; set; }
            [DisplayName("登入密碼")]
            public string LogInPsd { get; set; }
            public bool ErrorLog { get; set; }
            [DisplayName("異常登入")]
            public bool ErrorLogInLog { get; set; }
            [DisplayName("登入時狀態")]
            public bool AllowLogIn { get; set; }
        }
    }
}