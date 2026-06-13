using neoStockMasterv2.Data.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace neoStockMasterv2.Data.Services
{
    public class OrderService
    {
        public static User LoggedInUser { get; set; }
        private readonly string _filePath = "orders.json";
        private List<Order> _orders;

        public OrderService()
        {
            _orders = LoadOrders();
        }

        // ── Dosya İşlemleri

        private List<Order> LoadOrders()
        {
            if (!File.Exists(_filePath))
                return new List<Order>();

            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Order>>(jsonData) ?? new List<Order>();
        }

        private void SaveOrders()
        {
            var jsonData = JsonSerializer.Serialize(
                _orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }

        // ── CRUD İşlemleri

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            SaveOrders();
        }

        public bool DeleteOrder(string orderId)
        {
            var order = _orders.FirstOrDefault(o => o.ID == orderId);
            if (order == null) return false;

            // Stok iade
            var productService = new ProductService();
            foreach (var item in order.OrderContent)
                productService.AddStockToProduct(item.ProductName, item.Quantity);

            _orders.Remove(order);
            SaveOrders();
            return true;
        }

        public bool UpdateOrder(Order updatedOrder)
        {
            var order = _orders.FirstOrDefault(o => o.ID == updatedOrder.ID);
            if (order == null) return false;

            // ── Stok Dengeleme 
            var productService = new ProductService();

            var oldQtyMap = order.OrderContent
                .ToDictionary(x => x.ProductName, x => x.Quantity);
            var newQtyMap = updatedOrder.OrderContent
                .ToDictionary(x => x.ProductName, x => x.Quantity);

            // Silinen ürünler → tüm adedi stoğa iade
            foreach (var oldItem in oldQtyMap)
                if (!newQtyMap.ContainsKey(oldItem.Key))
                    productService.AddStockToProduct(oldItem.Key, oldItem.Value);

            // Yeni eklenen ürünler → stoktan düş
            foreach (var newItem in newQtyMap)
                if (!oldQtyMap.ContainsKey(newItem.Key))
                    productService.AddStockToProduct(newItem.Key, -newItem.Value);

            // Her iki listede de bulunanlar → adet farkını dengele
            foreach (var newItem in newQtyMap)
            {
                if (oldQtyMap.TryGetValue(newItem.Key, out int oldQty))
                {
                    int diff = oldQty - newItem.Value; // pozitif → iade, negatif → düş
                    if (diff != 0)
                        productService.AddStockToProduct(newItem.Key, diff);
                }
            }

            // ── Tüm Alanları Güncelle
            order.CustomerName        = updatedOrder.CustomerName;
            order.CustomerPhone       = updatedOrder.CustomerPhone;
            order.OrderDate           = updatedOrder.OrderDate;

            // Fiyatlandırma
            order.TotalPrice          = updatedOrder.TotalPrice;
            order.TotalDiscount       = updatedOrder.TotalDiscount;
            order.PayableAmount       = updatedOrder.PayableAmount;
            order.Tax                 = updatedOrder.Tax;           // ← önceden eksikti
            order.TaxPercentageVAT    = updatedOrder.TaxPercentageVAT;
            order.TaxPercentageSCT    = updatedOrder.TaxPercentageSCT;
            order.DiscountPercentage  = updatedOrder.DiscountPercentage;
            order.ExtraDiscountAmount = updatedOrder.ExtraDiscountAmount;
            order.CargoCost           = updatedOrder.CargoCost;

            // Durum / Kargo
            order.PayableStatues      = updatedOrder.PayableStatues;
            order.OrderStatues        = updatedOrder.OrderStatues;
            order.Cargo               = updatedOrder.Cargo;
            order.CargoTrackingNumber = updatedOrder.CargoTrackingNumber;

            // Ürün listesi
            order.OrderContent        = updatedOrder.OrderContent;

            SaveOrders();
            return true;
        }

        // ── Sorgular

        public List<Order> GetAllOrders() => _orders;

        public Order GetOrderById(string orderId) =>
            _orders.FirstOrDefault(o => o.ID == orderId);


        public List<Order> GetOrdersByLoggedInUser()
        {
            if (LoggedInUser == null)
            {
                MessageBox.Show("Lütfen giriş yapınız.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<Order>();
            }

            return _orders
                .Where(p => p.AddedBy.Equals(LoggedInUser.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
