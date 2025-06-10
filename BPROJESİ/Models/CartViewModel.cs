using System.Collections.Generic;
using System.Linq;

namespace BPROJESİ.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public Product Product { get; set; }
    }

    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(i => i.ProductPrice * i.Quantity);
            }
        }
    }
}
