using System;
using System.Collections.Generic;

namespace BPROJESİ.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string AdSoyad { get; set; }
        public string Adres { get; set; }
        public string OdemeTuru { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;
        public List<OrderItem> OrderItems { get; set; }
    }
}
