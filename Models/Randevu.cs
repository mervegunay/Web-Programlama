using Kuafor1.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Kuafor1.Models
{
    public class Randevu
    {
        public int RandevuId { get; set; } // Benzersiz kimlik
        [Required(ErrorMessage = "Randevu tarihi gereklidir.")]
        public DateTime RandevuTarih { get; set; }
        [Required(ErrorMessage = "Çalışan ID gereklidir.")]
        public int CalisanId { get; set; }
        [ValidateNever]
        public Calisan Calisan { get; set; }
        [Required(ErrorMessage = "Hizmet ID gereklidir.")]
        public int HizmetId { get; set; }
        [ValidateNever]
        public Hizmet Hizmet { get; set; }
        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        public string Telefon { get; set; }
        public int Sure { get; set; }
        public decimal Ucret { get; set; }
    }
}
