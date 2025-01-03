﻿using System.ComponentModel.DataAnnotations;

namespace Kuafor1.Models
{
    public class Hizmet
    {
        public int HizmetId { get; set; } // Hizmetin benzersiz kimliği

        [Required(ErrorMessage = "Hizmet adı gereklidir.")]
        public string HizmetAd { get; set; } = string.Empty; // Hizmetin adı

        [Required(ErrorMessage = "Hizmet fiyatı gereklidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat sıfırdan büyük olmalıdır.")]
        public decimal HizmetFiyat { get; set; } // Hizmetin ücreti

        [Required(ErrorMessage = "Hizmet süresi gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Süre en az 1 dakika olmalıdır.")]
        public int HizmetSure { get; set; } // Hizmetin süresi (dakika)

    }
}
