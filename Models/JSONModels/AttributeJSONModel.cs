using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models.JSONModels
{
    public class AttributeJSONModel
    {
        public int attributeId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? attributeValueId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string customAttributeValue { get; set; }
    }
}