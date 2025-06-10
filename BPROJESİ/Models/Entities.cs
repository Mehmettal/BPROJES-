using System.ComponentModel.DataAnnotations;

namespace BPROJESİ.Models
{
    public class Pet
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }

  
}