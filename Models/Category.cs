using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Category
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
    }   
}