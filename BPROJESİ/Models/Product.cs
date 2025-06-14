﻿namespace BPROJESİ.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }

        // Kullanıcı adı (kime ait)
        public string UserName { get; set; }
    }
}
