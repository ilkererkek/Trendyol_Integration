using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.ViewModels
{
    public class CategoryAttribute
    {
        public bool AllowCustom { get; set; }
        [Key]

        public int AttributeId { get; set; }
        [Required]
        public int SelectedId { get; set; }
        [Required]
        public string SelectedName{ get; set; }
        public string AttributeName { get; set; }
        public bool Required { get; set; }
        public List<AttributeValues> Values { get; set; }
    }
}