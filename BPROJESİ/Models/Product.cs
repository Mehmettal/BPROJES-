namespace BPROJESİ.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Yeni eklenen açıklama alanı
        public string? Description { get; set; }
    }

}
