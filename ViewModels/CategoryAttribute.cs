using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace Trendyol_Integration.ViewModels
{
    public class RequireWhenRequired : ValidationAttribute
    {

        //Custom validation to check if required attribute is selected or not.
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CategoryAttribute)validationContext.ObjectInstance;
            if (model.Required&&model.AllowCustom&&string.IsNullOrEmpty(model.SelectedValue))
            {
                return new ValidationResult("Value is required.");
            }
            else if(model.Required&&!model.AllowCustom&&model.SelectedId==0)
            {
                return new ValidationResult("Value is required.");
            }
            return ValidationResult.Success;
           
        }
    }


    
    public class CategoryAttribute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RequireWhenRequired]
        public bool Required { get; set; }
        public bool AllowCustom { get; set; }
        public int SelectedId { get; set; }
        public string SelectedValue { get; set; }
        public List<AttributeValue> Values { get; set; }
        public CategoryAttribute(int id, string name, bool required, bool allowCustom)
        {
            Id = id;
            Name = name;
            Required = required;
            AllowCustom = allowCustom;
            Values = new List<AttributeValue>();
        }
        public CategoryAttribute()
        {
        }
    }
}