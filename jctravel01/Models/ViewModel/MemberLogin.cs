using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models.ViewModel
{
    public class MemberLogin
    {
        [DisplayName("公司編號")]
        [Required(ErrorMessage="請輸入公司編號")]
        [RegularExpression(@"[0-9]*",ErrorMessage="公司編號不正確")]
        [StringLength(5)]
        public string CompanyNo { get; set; }
        [DisplayName("使用者帳號")]
        [Required(ErrorMessage="請輸入帳號")]
        [RegularExpression(@"^[A-Z]{1,1}[0-9]{4,4}",ErrorMessage="帳號不正確")]
        public string EmpNo { get; set; }
        [DisplayName("使用者密碼")]
        [StringLength(8,ErrorMessage="密碼必需為6-8個字",MinimumLength=6)]
        [RegularExpression(@"[a-zA-Z]{1,}[0-9]{1,}", ErrorMessage = "密碼不正確")]
        [Required(ErrorMessage="請輸入密碼")]
        public string PassWord { get; set; }
        [DisplayName("確認密碼")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        [Compare("PassWord",ErrorMessage="與第一次輸入不相符")]
        public string PassWord2 { get; set; }
    }
}