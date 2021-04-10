using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.ViewModels
{
    public class AttributeValue
    {
        public int Id { get; set; }
        public string Value { get; set; }

        public AttributeValue(int id, string value)
        {
            Id = id;
            Value = value;
        }
        public AttributeValue()
        {

        }
    }
}