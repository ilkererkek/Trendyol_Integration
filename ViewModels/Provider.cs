using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.ViewModels
{
    public class Provider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Provider(int Id, string Name)
        {
            this.Id = Id;
            this.Name = Name;
        }
    }   
}