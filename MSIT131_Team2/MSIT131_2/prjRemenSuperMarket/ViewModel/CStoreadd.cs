using Microsoft.AspNetCore.Http;
using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    public class CStoreAdd
    {
        public CStoreAdd()
        {
            RamenStore = new RamenStore();
        }

        public RamenStore RamenStore { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Picture { get; set; }

        public string LogoBase64
        {
            get 
            {
                if(LogoBytes != null)
                    return Convert.ToBase64String(LogoBytes);

                return "";
            } 
        }

        public string PictureBase64
        {
            get => PictureBytes != null ? Convert.ToBase64String(PictureBytes) : "";
        }

        public byte[] LogoBytes
        {
            get => RamenStore.Logo;
            set => RamenStore.Logo = value;
        }

        public byte[] PictureBytes
        {
            get => RamenStore.Pictrue;
            set => RamenStore.Pictrue = value;
        }

        public int? MemberId
        {
            get => RamenStore.MemberId;
            set => RamenStore.MemberId = value;
        }

        public int RamenStoreId
        {
            get => RamenStore.RamenStoreId;
            set => RamenStore.RamenStoreId = value;
        }

        [DisplayName("店家名稱")]
        [Required(ErrorMessage =("未輸入"))]
        public string StoreName
        {
            get => RamenStore.StoreName;
            set => RamenStore.StoreName = value;
        }

        [DisplayName("店家介紹")]
        [Required(ErrorMessage = ("未輸入"))]
        public string Introduce
        {
            get => RamenStore.Introduce;
            set => RamenStore.Introduce = value;
        }

        [DisplayName("城市")]
        [Required(ErrorMessage =("未輸入"))]
        public int? CityId
        {
            get => RamenStore.CityId;
            set => RamenStore.CityId = value;
        }

        [DisplayName("區域")]
        [Required(ErrorMessage =("未輸入"))]
        public int? DistrictId
        {
            get => RamenStore.DistrictId;
            set => RamenStore.DistrictId = value;
        }

        [DisplayName("地址")]
        [Required(ErrorMessage =("未輸入"))]
        public string Address
        {
            get => RamenStore.Address;
            set => RamenStore.Address = value;
        }

        [DisplayName("電話")]
        [Required(ErrorMessage =("未輸入"))]
        public string Phone
        {
            get => RamenStore.Phone;
            set => RamenStore.Phone = value;
        }
    }
}
