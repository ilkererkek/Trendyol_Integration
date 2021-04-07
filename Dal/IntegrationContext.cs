using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Trendyol_Integration.Models;

namespace Trendyol_Integration.Dal
{
    public class IntegrationContext:DbContext
    {
        public IntegrationContext() : base("IntegrationDB")
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Models.Attribute> Attributes { get; set; }
    }
}