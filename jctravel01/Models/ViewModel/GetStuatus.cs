using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jctravel01.Models;

namespace jctravel01.Models.ViewModel
{
    public static class GetStuatus
    {
        public static List<Status> SetStatus = new List<Status>{
         new Status(){
            Key = 1,
            Value = "上架"
            },
            new Status(){
                Key =2,
                Value ="下架"
            }
        };
        public static SelectList GetStatus(int? now =2)
        {
            return new SelectList(SetStatus, "Key", "Value",now);
        }
        public static SelectList DefualStatus()
        {
            return new SelectList(SetStatus, "Key", "Value");
        }
        public static string ValidaStatus(int? status)
        {
            if (status == 1)
            {
                return "上架";
            }
            else
            {
                return "下架";
            }
        }
        
    }
}