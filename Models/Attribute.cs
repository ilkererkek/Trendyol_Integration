using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Attribute
    {

        public int AttributeId { get; set; }
        public int AttributeCode { get; set; }
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public int AttributeValueId { get; set; }
        public virtual Category Category { get; set; }
    }
}