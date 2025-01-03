﻿using System.ComponentModel.DataAnnotations;

namespace Kuafor1.Models
{
    public class Uye
    {
        [Key]
        public int UyeId { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public string KullaniciAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası gereklidir.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Telefon numarası 10 haneli olmalıdır.")]
        public string Telefon { get; set; } = string.Empty;
    }
}
