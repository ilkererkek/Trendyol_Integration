using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.ViewModels
{
    public class SupplierAdress
    {
        public int id { get; set; }
        public string city { get; set; }
        public string district { get; set; }

        public SupplierAdress(int id, string city, string district)
        {
            this.id = id;
            this.city = city;
            this.district = district;
        }
    }
}