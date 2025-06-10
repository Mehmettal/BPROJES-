using Microsoft.AspNetCore.Identity;
using System;

namespace BPROJESİ.Models
{
   

    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }  // Buraya ekle
        public string? Ad { get; set; }
        public string? Soyad { get; set; }
        public DateTime? KayitTarihi { get; set; }  
        public DateTime BirthDate { get; set; }
        


        // İstediğin diğer özel alanlar...
    }


}
