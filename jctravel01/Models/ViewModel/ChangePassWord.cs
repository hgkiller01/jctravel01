using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public class ChangePassWord
    {
        public int empIndex { get; set; }
        [DisplayName("密碼")]
        [RegularExpression(@"[a-zA-Z]{1,}[0-9]{1,}", ErrorMessage = "密碼必需為英文與數字混合")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "密碼長度必需為8-6個字")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        [DisplayName("確認密碼")]
        [Compare("PassWord", ErrorMessage = "與第一次輸入不相符")]
        [DataType(DataType.Password)]
        public string RepeatPass { get; set; }
        public string Place { get; set; }
    }
}