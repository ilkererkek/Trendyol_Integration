using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Trendyol_Integration.Models
{
    public class Sale
    {
        public string ShipmentAddress { get; set; }
        public string InvoiceAddress { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalPrice { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string CargoTrackingNumber { get; set; }
        public string CargoTrackingLink { get; set; }
        public string CargoSenderNumber { get; set; }
        public string CargoProviderName { get; set; }
        public DateTime OrderDate { get; set; }
        public string TcIdentityNumber { get; set; }
        public string ShipmentPackageStatus { get; set; }
        public DateTime EstimatedDeliveryEndDate { get; set; }
        public DateTime EstimatedDeliveryStartDate { get; set; }

        public List<Line> Lines { get; set; }

        public Sale(string shipmentAddress, string ınvoiceAddress, decimal totalDiscount, decimal totalPrice, string customerFirstName, string customerLastName, string customerEmail, string cargoTrackingNumber, string cargoTrackingLink, string cargoSenderNumber, string cargoProviderName, DateTime orderDate, string tcIdentityNumber, string shipmentPackageStatus, DateTime estimatedDeliveryEndDate, DateTime estimatedDeliveryStartDate)
        {
            ShipmentAddress = shipmentAddress;
            InvoiceAddress = ınvoiceAddress;
            TotalDiscount = totalDiscount;
            TotalPrice = totalPrice;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerEmail = customerEmail;
            CargoTrackingNumber = cargoTrackingNumber;
            CargoTrackingLink = cargoTrackingLink;
            CargoSenderNumber = cargoSenderNumber;
            CargoProviderName = cargoProviderName;
            OrderDate = orderDate;
            TcIdentityNumber = tcIdentityNumber;
            ShipmentPackageStatus = shipmentPackageStatus;
            EstimatedDeliveryEndDate = estimatedDeliveryEndDate;
            EstimatedDeliveryStartDate = estimatedDeliveryStartDate;
            Lines = new List<Line>();
        }
    }
}