using Kuafor1.Models; // Veritabanı modelleri için
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // LINQ işlemleri için

namespace Kuafor1.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor - ApplicationDbContext ile veritabanına bağlanıyoruz
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Giriş işlemleri
        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Giris(string kullaniciAdi, string sifre)
        {
            string adminKullaniciAdi = "B201210017@sakarya.edu.tr";
            string adminSifre = "sau";

            if (kullaniciAdi == adminKullaniciAdi && sifre == adminSifre)
            {
                return RedirectToAction("Islemler");
            }
            else if (kullaniciAdi != adminKullaniciAdi)
            {
                ViewBag.Mesaj = "Admin bulunamadı.";
            }
            else if (sifre != adminSifre)
            {
                ViewBag.Mesaj = "Şifre yanlış.";
            }

            return View();
        }

        // Admin işlemleri ana sayfası
        public IActionResult Islemler()
        {
            return View();
        }

        // Çıkış işlemleri
        public IActionResult Cikis()
        {
            return RedirectToAction("Giris");
        }

        // Hizmet İşlemleri Ana Sayfası
        public IActionResult HizmetIslemleri()
        {
            var hizmetler = _context.Hizmetler.ToList(); // DB'den hizmetleri çekiyoruz
            return View(hizmetler); // Görünüme hizmet listesini gönderiyoruz
        }

        // Yeni Hizmet Ekleme
        [HttpPost]
        public IActionResult HizmetEkle(string hizmetAd, decimal hizmetFiyat, int hizmetSure)
        {
            if (string.IsNullOrWhiteSpace(hizmetAd) || hizmetFiyat <= 0 || hizmetSure <= 0)
            {
                TempData["Mesaj"] = "Geçersiz hizmet bilgileri. Lütfen tüm alanları doğru doldurun.";
                return RedirectToAction("HizmetIslemleri");
            }

            var yeniHizmet = new Hizmet
            {
                HizmetAd = hizmetAd,
                HizmetFiyat = hizmetFiyat,
                HizmetSure = hizmetSure
            };

            _context.Hizmetler.Add(yeniHizmet); // DB'ye yeni hizmet ekleniyor
            _context.SaveChanges(); // Değişiklikler kaydediliyor

            TempData["Mesaj"] = "Hizmet başarıyla eklendi.";
            return RedirectToAction("HizmetIslemleri");
        }

        // Hizmet Silme
        public IActionResult HizmetSil(int id)
        {
            var hizmet = _context.Hizmetler.Find(id); // ID ile hizmeti buluyoruz
            if (hizmet == null)
            {
                TempData["Mesaj"] = "Silinmek istenen hizmet bulunamadı.";
                return RedirectToAction("HizmetIslemleri");
            }

            _context.Hizmetler.Remove(hizmet); // Hizmet siliniyor
            _context.SaveChanges(); // Değişiklikler kaydediliyor

            TempData["Mesaj"] = "Hizmet başarıyla silindi.";
            return RedirectToAction("HizmetIslemleri");
        }

        // Çalışan İşlemleri Ana Sayfası
        public IActionResult CalisanIslemleri()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanUzmanliklar) // Uzmanlık alanlarını da dahil et
                .ToList();
            var hizmetler = _context.Hizmetler.ToList(); // Hizmetleri al
            ViewBag.Hizmetler = hizmetler; // Görünüme hizmetleri gönder
            return View(calisanlar);
        }

        // Yeni Çalışan Ekleme
        [HttpPost]
        public IActionResult CalisanEkle(string calisanAd, string calisanUygunluk, List<int> uzmanlikIds)
        {
            if (string.IsNullOrWhiteSpace(calisanAd) || string.IsNullOrWhiteSpace(calisanUygunluk) || uzmanlikIds == null || !uzmanlikIds.Any())
            {
                TempData["Mesaj"] = "Geçersiz çalışan bilgileri. Lütfen tüm alanları doğru doldurun ve en az bir uzmanlık seçin.";
                return RedirectToAction("CalisanIslemleri");
            }

            var uzmanliklar = _context.Hizmetler.Where(h => uzmanlikIds.Contains(h.HizmetId)).ToList();

            var yeniCalisan = new Calisan
            {
                CalisanAd = calisanAd,
                CalisanUygunluk = calisanUygunluk,
                CalisanUzmanliklar = uzmanliklar
            };

            _context.Calisanlar.Add(yeniCalisan);
            _context.SaveChanges();

            TempData["Mesaj"] = "Çalışan başarıyla eklendi.";
            return RedirectToAction("CalisanIslemleri");
        }

        // Çalışan Silme
        public IActionResult CalisanSil(int id)
        {
            var calisan = _context.Calisanlar
                .Include(c => c.CalisanUzmanliklar) // Uzmanlıkları ile birlikte al
                .FirstOrDefault(c => c.CalisanId == id);

            if (calisan == null)
            {
                TempData["Mesaj"] = "Silinmek istenen çalışan bulunamadı.";
                return RedirectToAction("CalisanIslemleri");
            }

            _context.Calisanlar.Remove(calisan);
            _context.SaveChanges();

            TempData["Mesaj"] = "Çalışan başarıyla silindi.";
            return RedirectToAction("CalisanIslemleri");
        }

        // Üyeleri Listeleme
        public IActionResult UyeListele()
        {
            var uyeler = _context.Uyeler.ToList(); // Veritabanından tüm üyeleri al
            return View(uyeler); // Görünüme gönder
        }

        /// Randevu Listeleme Sayfası
        public IActionResult RandevuListele()
        {
            var randevular = _context.Randevular
                                     .Include(r => r.Hizmet)
                                     .Include(r => r.Calisan)
                                     .ToList();

            return View(randevular);
        }

    }
}
