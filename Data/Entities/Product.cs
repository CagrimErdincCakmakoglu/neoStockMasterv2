using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neoStockMasterv2.Data.Entities
{
    public class Product
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string CostCurrency { get; set; }
        public decimal Price { get; set; }
        public string PriceCurrency { get; set; }
        public int Stock { get; set; }
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; }
    }
}
