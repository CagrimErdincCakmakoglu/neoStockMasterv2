using neoStockMasterv2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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


        private List<Order> LoadOrders()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Order>();
            }

            var jsonData = File.ReadAllText(_filePath);
            return System.Text.Json.JsonSerializer.Deserialize<List<Order>>(jsonData) ?? new List<Order>();

        }


        private void SaveOrders()
        {
            var jsonData = System.Text.Json.JsonSerializer.Serialize(_orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }


        public void AddOrder(Order order)
        {
            _orders.Add(order);
            SaveOrders();
        }


        public bool DeleteOrder(string orderId)
        {
            var order = _orders.FirstOrDefault(o => o.ID == orderId);
            if (order != null)
            {
                _orders.Remove(order);
                SaveOrders();
                return true;
            }
            return false;
        }


        public bool UpdateOrder(Order updatedOrder)
        {
            var order = _orders.FirstOrDefault(o => o.ID == updatedOrder.ID);
            if (order != null)
            {
                order.CustomerName = updatedOrder.CustomerName;
                order.CustomerPhone = updatedOrder.CustomerPhone;
                order.OrderDate = updatedOrder.OrderDate;
                order.TotalPrice = updatedOrder.TotalPrice;
                order.TotalDiscount = updatedOrder.TotalDiscount;
                order.PayableAmount = updatedOrder.PayableAmount;
                order.OrderContent = updatedOrder.OrderContent;
                order.PayableStatues = updatedOrder.PayableStatues;
                order.OrderStatues = updatedOrder.OrderStatues;
                order.Cargo = updatedOrder.Cargo;
                order.CargoTrackingNumber = updatedOrder.CargoTrackingNumber;
                SaveOrders();
                return true;
            }
            return false;
        }


        public List<Order> GetAllOrders()
        {
            return _orders;
        }


        public Order GetOrderById(string orderId)
        {
            var orders = GetAllOrders(); // orders.json'dan tüm siparişleri al
            return orders.FirstOrDefault(order => order.ID == orderId);
        }

        public List<Order> GetOrdersByLoggedInUser()
        {
            if (LoggedInUser == null)
            {
                MessageBox.Show("Lütfen giriş yapınız.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return new List<Order>(); // Kullanıcı giriş yapmamışsa boş liste döndür
            }

            var orders = _orders;

            var userOrders = orders.Where(p => p.AddedBy.Equals(LoggedInUser.Name, StringComparison.OrdinalIgnoreCase)).ToList();

            return userOrders;
        }
    }
}
