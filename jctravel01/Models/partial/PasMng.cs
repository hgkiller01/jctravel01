using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(PasMngMD))]
    public partial class PasMng
    {
        public class PasMngMD
        {
            public long psdindex { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [ScaffoldColumn(false)]
            public int EmpIndex { get; set; }
            [ScaffoldColumn(false)]
            public string oldpsd { get; set; }
            [DisplayName("密碼")]
            [RegularExpression(@"[A-Za-z]{1,}[0-9]{1,}",ErrorMessage="密碼必需為英文與數字混合")]
            [StringLength(8,MinimumLength=6,ErrorMessage="{0}長度必需為{1}到{2}")]
            public string newpsd { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
        }
    }
}