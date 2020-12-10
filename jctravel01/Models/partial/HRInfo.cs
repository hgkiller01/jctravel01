using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace jctravel01.Models
{
    [MetadataType(typeof(HRInfoMD))]
    public partial class HRInfo
    {
        public class HRInfoMD
        {
            //TODO 今日進度
            public int EmpIndex { get; set; }
            [DisplayName("員工編號")]
            public string EmpNo { get; set; }
            [ScaffoldColumn(false)]
            public string CompanyNo { get; set; }
            [DisplayName("部門")]
            [Required(ErrorMessage = "{0}必填")]
            public int Dep_Index { get; set; }
            [DisplayName("職等")]
            [Required(ErrorMessage = "{0}必填")]
            public int JobLevel_Index { get; set; }
            [DisplayName("群組")]
            [Required(ErrorMessage = "{0}必填")]
            public int JobGruop_Index { get; set; }
            [DisplayName("英文姓名")]
            [StringLength(10)]
            public string EmpEnName { get; set; }
            [DisplayName("中文姓名")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(20)]
            public string EmpName { get; set; }
            [DisplayName("主管")]
            public int JobManager { get; set; }
            [DisplayName("職稱")]
            [Required(ErrorMessage = "{0}必填")]
            public string JobTitle { get; set; }
            [DisplayName("身分證字號(外國人則填護照號碼或居留證號)")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(20)]
            public string EmpID { get; set; }
            [DisplayName("生日")]
            [Required(ErrorMessage = "{0}必填")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DataType(DataType.Date)]
            public System.DateTime EmpBday { get; set; }
            [DisplayName("公司電話")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(12)]
            [RegularExpression(@"[0-9\-]*")]
            public string ComPhone { get; set; }
            [DisplayName("分機")]
            [StringLength(10)]
            public string ComExt { get; set; }
            [DisplayName("公司E-mail")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(50)]
            [DataType(DataType.EmailAddress)]
            public string ComEmail { get; set; }
            [DisplayName("戶籍地址")]
            [Required(ErrorMessage = "{0}必填")]
            [DataType(DataType.MultilineText)]
            public string HomeAddres { get; set; }
            [DisplayName("通訊地址")]
            [Required(ErrorMessage = "{0}必填")]
            [DataType(DataType.MultilineText)]
            public string ContactAddres { get; set; }
            [DisplayName("私人E-mail")]
            [DataType(DataType.EmailAddress)]
            public string PrivateMail { get; set; }
            [DisplayName("非公司電話")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(12)]
            [RegularExpression(@"[0-9\-]*")]
            public string PrivatePhone { get; set; }
            [DisplayName("緊急聯絡人")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(20)]
            public string UrgentName { get; set; }
            [DisplayName("與緊急連絡人關係")]
            [Required(ErrorMessage = "{0}必填")]
            [StringLength(12)]
            public string RelationUrgent { get; set; }
            [DisplayName("緊急連絡人電話")]
            [StringLength(12)]
            [Required(ErrorMessage = "{0}必填")]
            [RegularExpression(@"[0-9\-]*")]
            public string UrgentPhone { get; set; }
            [DisplayName("在職狀態")]
            [Required(ErrorMessage = "{0}必填")]
            public int OnJobStatus { get; set; }
            [DisplayName("正式上班日期")]
            [Required(ErrorMessage = "{0}必填")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DataType(DataType.Date)]
            public System.DateTime OnBoardDate { get; set; }
            [DisplayName("離職日期")]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
            [DataType(DataType.Date)]
            public Nullable<System.DateTime> ResignDate { get; set; }
            [DisplayName("建立者")]
            public int CreateBy { get; set; }
            [DisplayName("建立時間")]
            public System.DateTime CreateBy_Time { get; set; }
            [DisplayName("修改者")]
            public int UpdateBy { get; set; }
            [DisplayName("修改時間")]
            public System.DateTime UpdateBy_Time { get; set; }
            [DisplayName("允許登入")]
            public bool AllowLogIn { get; set; }
        }
    }
    public class IDValidator
    {
        public static bool IDChk(string EmpID)
        {
            List<string> FirstEng = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "W", "Z", "I", "O" };
            string aa = EmpID.ToUpper();
            bool chackFirstEnd = false;
            if (aa.Trim().Length == 10)
            {
                byte firstNo = Convert.ToByte(aa.Trim().Substring(1, 1));
                if (firstNo > 2 || firstNo < 1)
                {
                    return false;
                }
                else
                {
                    int x;
                    for (x = 0; x < FirstEng.Count; x++)
                    {
                        if (aa.Substring(0, 1) == FirstEng[x])
                        {
                            aa = string.Format("{0}{1}", x + 10, aa.Substring(1, 9));
                            chackFirstEnd = true;
                            break;
                        }

                    }
                    if (!chackFirstEnd)
                        return false;

                    int i = 1;
                    int ss = int.Parse(aa.Substring(0, 1));
                    while (aa.Length > i)
                    {
                        ss = ss + (int.Parse(aa.Substring(i, 1)) * (10 - i));
                        i++;
                    }
                    aa = ss.ToString();
                    if (EmpID.Substring(9, 1) == "0")
                    {
                        if (aa.Substring(aa.Length - 1, 1) == "0")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (EmpID.Substring(9, 1) == (10 - int.Parse(aa.Substring(aa.Length - 1, 1))).ToString())
                        {

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {

                return false;
            }
        }
        //回傳1 代表字數不到10  
        //回傳2代表第二碼非1,2  
        //回傳3 代表首碼有誤  
        //回傳4代表檢查碼不對  

    }
}