using Microsoft.AspNetCore.Identity;
using System;

namespace BPROJESİ.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }  // İsim
        public string LastName { get; set; }   // Soyisim
        public DateTime? BirthDate { get; set; }
        // Doğum tarihi
        // İstersen telefon numarası ve e-posta IdentityUser'da zaten var, tekrar eklemeye gerek yok.
    }
}
