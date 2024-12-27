using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kuafor1.Models
{
    public class Calisan
    {
        public int CalisanId { get; set; } // Çalışanın benzersiz kimliği

        [Required(ErrorMessage = "Çalışan adı gereklidir.")]
        public string CalisanAd { get; set; } = string.Empty; // Çalışanın adı

        [Required(ErrorMessage = "Uzmanlık alanları gereklidir.")]
        public List<Hizmet> CalisanUzmanliklar { get; set; } = new List<Hizmet>(); // Çalışanın uzmanlık alanları

        [Required(ErrorMessage = "Uygunluk saatleri gereklidir.")]
        public string CalisanUygunluk { get; set; } = string.Empty; // Çalışanın uygunluk saatleri
    }
}
