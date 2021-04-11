using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Line
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public virtual Product Product { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Price { get; set; }
        public int VatBaseAmount { get; set; }

        public Line(int quantity, string productName,decimal amount, decimal discount, decimal price, int vatBaseAmount)
        {
            Quantity = quantity;
            ProductName = productName;
            Amount = amount;
            Discount = discount;
            Price = price;
            VatBaseAmount = vatBaseAmount;
        }
    }
}