using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CRegister
    {
        public CRegister cregister{ get; set; }
        [DisplayName("姓名")]
        [Required(ErrorMessage = "姓名是必填欄位")]
        public string Name { get; set; }
        [DisplayName("電話")]
        [Required(ErrorMessage = "電話是必填欄位")]
        public string Phone { get; set; }
        [DisplayName("縣市")]
        [Required(ErrorMessage = "縣市是必填欄位")]
        public string CityName { get; set; }
        [DisplayName("區")]
        [Required(ErrorMessage = "區是必填欄位")]
        public string DistrictName { get; set; }
        [DisplayName("地址")]
        [Required(ErrorMessage = "地址是必填欄位")]
        public string Address { get; set; }
        [DisplayName("電子信箱")]
        [Required(ErrorMessage = "電子信箱是必填欄位")]
        public string EMail { get; set; }
        [DisplayName("密碼")]
        [Required(ErrorMessage = "密碼是必填欄位")]
        public string Password { get; set; }
        [DisplayName("出生年月日")]
        [Required(ErrorMessage = "出生年月日是必填欄位")]
        public DateTime Birthday { get; set; }
    }
}
