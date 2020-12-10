using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jctravel01.Models.ViewModel
{
    public class GetImgSearch
    {
        public List<SearchType> search { get; set; }
        public GetImgSearch()
        {
            search = new List<SearchType>(){
                new SearchType(){
                    searchType =1,
                    searchText = "中文名稱"
                },
                new SearchType(){
                    searchType =2,
                    searchText ="英文名稱"
                },
                new SearchType(){
                    searchType =3,
                    searchText ="元件代碼"
                },
                new SearchType(){
                    searchType =4,
                    searchText ="城市代碼"
                }
            };
        }
    }
}