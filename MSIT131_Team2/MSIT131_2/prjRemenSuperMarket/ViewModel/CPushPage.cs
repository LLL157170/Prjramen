using Microsoft.AspNetCore.Http;
using prjRemenSuperMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjRemenSuperMarket.ViewModel
{
    
    public class CPushPage
    {
        public RamenStore data1 { get; set; }

        public RamenProductInfo data2 { get; set; }

        public string StoreName { get {
                return data1.StoreName;

            } }


        public string Introduce { get {
                return data1.Introduce;
            } }        
        public string ProductName {
            get
            {
                return data2.ProductName;
            }
        }
    
        public string ProductPicture {
            get
            {
                return Convert.ToBase64String(data1.Pictrue);
            }
        }
    
    }
}
