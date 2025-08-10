using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neoStockMasterv2.Data.Entities
{
    public class Order
    {
        public string ID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal TaxPercentageVAT { get; set; }
        public decimal TaxPercentageSCT { get; set; }
        public List<OrderDetail> OrderContent { get; set; }
        public string PayableStatues { get; set; }
        public string OrderStatues { get; set; }
        public string Cargo { get; set; }
        public string CargoTrackingNumber { get; set; }
        public decimal CargoCost { get; set; }
        public string AddedBy { get; set; }

        public Order()
        {
            OrderContent = new List<OrderDetail>(); // Listeyi başlatıyoruz
        }

        public class OrderDetail
        {
            public string ProductName { get; set; }
            public decimal OrderPrice { get; set; }
            public int Quantity { get; set; }
        }
    }
}
