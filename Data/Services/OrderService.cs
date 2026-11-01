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
        private static readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "orders.json");
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
            var jsonData = JsonSerializer.Serialize(_orders, new JsonSerializerOptions { WriteIndented = true });
            string tempPath = _filePath + ".tmp";
            File.WriteAllText(tempPath, jsonData);
            File.Move(tempPath, _filePath, overwrite: true);
        }

        // ── CRUD İşlemleri

        public void AddOrder(Order order)
        {
            _orders.Add(order);
            SaveOrders();

            ActivityLogService.Log(ActivityType.OrderAdded, new Dictionary<string, string>
            {
                ["Customer"] = order.CustomerName ?? "-",
                ["Phone"] = order.CustomerPhone ?? "-",
                ["Items"] = ActivityLogService.FormatItems(order.OrderContent),
                ["Amount"] = ActivityLogService.FormatAmounts(order.PayableAmount),
                ["Status"] = order.OrderStatues ?? "-",
                ["Payment"] = order.PayableStatues ?? "-"
            });
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
            ActivityLogService.Log(ActivityType.OrderDeleted, new Dictionary<string, string>
            {
                ["Customer"] = order.CustomerName ?? "-",
                ["Items"] = ActivityLogService.FormatItems(order.OrderContent),
                ["Amount"] = ActivityLogService.FormatAmounts(order.PayableAmount)
            });
            return true;
        }

        public bool UpdateOrder(Order updatedOrder)
        {
            var order = _orders.FirstOrDefault(o => o.ID == updatedOrder.ID);
            if (order == null) return false;

            // ── Stok Dengeleme 
            var productService = new ProductService();

            var oldQtyMap = order.OrderContent
                .GroupBy(x => x.ProductName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity), StringComparer.OrdinalIgnoreCase);

            var newQtyMap = updatedOrder.OrderContent
                .GroupBy(x => x.ProductName, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity), StringComparer.OrdinalIgnoreCase);

            // Artan / yeni eklenen adetler için stok yeterli mi?
            var extraNeeded = newQtyMap
                .Select(n => (ProductName: n.Key,
                              Quantity: n.Value - (oldQtyMap.TryGetValue(n.Key, out int o) ? o : 0)))
                .Where(x => x.Quantity > 0)
                .ToList();

            string? stockError = productService.CheckStock(extraNeeded);
            if (stockError != null)
            {
                MessageBox.Show(stockError, LanguageService.T("Yetersiz Stok", "Insufficient Stock"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

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

            string oldAmount = ActivityLogService.FormatAmounts(order.PayableAmount);
            string oldStatus = order.OrderStatues ?? "-";
            string oldItems = ActivityLogService.FormatItems(order.OrderContent);

            // ── Tüm Alanları Güncelle
            order.CustomerName        = updatedOrder.CustomerName;
            order.CustomerPhone       = updatedOrder.CustomerPhone;
            order.OrderDate           = updatedOrder.OrderDate;

            // Fiyatlandırma
            order.TotalPrice          = updatedOrder.TotalPrice;
            order.TotalDiscount       = updatedOrder.TotalDiscount;
            order.PayableAmount       = updatedOrder.PayableAmount;
            order.Tax                 = updatedOrder.Tax;           
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
            ActivityLogService.Log(ActivityType.OrderUpdated, new Dictionary<string, string>
            {
                ["Customer"] = order.CustomerName ?? "-",
                ["Items"] = ActivityLogService.Change(oldItems, ActivityLogService.FormatItems(order.OrderContent)),
                ["Amount"] = ActivityLogService.Change(oldAmount, ActivityLogService.FormatAmounts(order.PayableAmount)),
                ["Status"] = ActivityLogService.Change(oldStatus, order.OrderStatues ?? "-")
            });
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
                MessageBox.Show(LanguageService.T("Lütfen giriş yapınız.", "Please log in."),
                    LanguageService.T("Uyarı", "Warning"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<Order>();
            }

            return _orders
                .Where(p => p.AddedBy.Equals(LoggedInUser.Name, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void RenameProductInOrders(string oldName, string newName, string userName)
        {
            bool changed = false;

            foreach (var order in _orders.Where(o => string.Equals(o.AddedBy, userName, StringComparison.OrdinalIgnoreCase)))
            {
                foreach (var detail in order.OrderContent.Where(d => string.Equals(d.ProductName, oldName, StringComparison.OrdinalIgnoreCase)))
                {
                    detail.ProductName = newName;
                    changed = true;
                }
            }

            if (changed) SaveOrders();
        }
    }
}
