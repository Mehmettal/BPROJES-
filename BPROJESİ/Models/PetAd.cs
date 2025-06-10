namespace BPROJESİ.Models
{
    public class PetAd
    {
        public int Id { get; set; }

        public string AdTitle { get; set; }
        public string PetType { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Size { get; set; }
        public bool Vaccinated { get; set; }
        public bool Sterilized { get; set; }
        public bool Microchipped { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string AdType { get; set; }
        public decimal? Price { get; set; }
        public string PhotoUrls { get; set; } // Fotoğraf url'leri virgülle ayrılmış string olarak saklanacak
        public string Description { get; set; }
        public string ContactPhone { get; set; }

        public string? PhotoPaths { get; set; } // Virgülle ayrılmış fotoğraf yolları

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
