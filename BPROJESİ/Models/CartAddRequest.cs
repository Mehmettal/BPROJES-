public class CartAddRequest
{
    public int ProductId { get; set; }      // Ürün ID'si (zorunlu)
    public string Name { get; set; }        // Ürün adı (zorunlu olabilir, bazen sadece ID yeter)
    public decimal Price { get; set; }      // Ürün fiyatı (backend kontrolü için)
    public int Quantity { get; set; } = 1;  // Sepete eklenecek adet, default 1
    public string? PhotoPaths { get; set; }   // Görsel URL'si (isteğe bağlı, kullanıcıya gösterim için)
    public string ImageUrl { get; set; }

}
