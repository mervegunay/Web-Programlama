using Kuafor1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kuafor1.Controllers
{
    public class UyeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UyeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Üye Giriş Sayfası
        public IActionResult Giris()
        {
            if (TempData["HataMesaji"] != null)
            {
                ViewBag.HataMesaji = TempData["HataMesaji"];
            }
            return View();
        }

        // Yeni Üye Oluşturma Sayfası
        public IActionResult KayitOl()
        {
            return View();
        }

        // Yeni Üye Kaydı
        [HttpPost]
        public IActionResult KayitOl(Uye yeniUye)
        {
            if (yeniUye.Telefon.Length != 10 || !long.TryParse(yeniUye.Telefon, out _))
            {
                ModelState.AddModelError("Telefon", "Telefon numarası tam olarak 10 haneli olmalıdır.");
                return View(yeniUye);
            }

            if (ModelState.IsValid)
            {
                _context.Uyeler.Add(yeniUye);
                _context.SaveChanges();

                // Başarılı mesajını TempData ile aktar
                TempData["Mesaj"] = "Üye olma işlemi başarılı.";
                return RedirectToAction("Giris");
            }
            return View(yeniUye);
        }

        // Üye Giriş İşlemi (POST)
        // Üye Giriş İşlemi
        [HttpPost]
        public IActionResult Giris(string kullaniciAdi, string telefon)
        {
            // Kullanıcı adı ve telefon numarasını kontrol et
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(telefon) || telefon.Length != 10 || !long.TryParse(telefon, out _))
            {
                ViewBag.Mesaj = "Kullanıcı adı ve telefon numarası doğru formatta olmalıdır.";
                return View();
            }

            // Kullanıcı adı ve telefon numarasının eşleşmesini kontrol et
            var uye = _context.Uyeler.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi && u.Telefon == telefon);
            if (uye == null)
            {
                ViewBag.Mesaj = "Kullanıcı adı ve telefon numarası eşleşmiyor.";
                return View();
            }

            // Başarılı giriş durumunda randevularını listeleme sayfasına yönlendir
            return RedirectToAction("Randevularim", new { telefon = uye.Telefon });
        }


        // Randevularım Sayfası (GET)
        [HttpGet]
        public IActionResult Randevularim(string telefon)
        {
            if (string.IsNullOrEmpty(telefon))
            {
                TempData["HataMesaji"] = "Telefon bilgisi eksik, lütfen yeniden giriş yapın.";
                return RedirectToAction("Giris");
            }

            ViewBag.Telefon = telefon;
            ViewBag.Calisanlar = _context.Calisanlar.ToList();
            ViewBag.Hizmetler = _context.Hizmetler.ToList();

            var randevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Calisan)
                .Where(r => r.Telefon == telefon)
                .ToList();

            return View(randevular);
        }

        [HttpPost]
        public IActionResult RandevuEkle(Randevu randevu)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
                return BadRequest(new { message = "Model doğrulama hatası", errors });
            }

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.HizmetId == randevu.HizmetId);
            if (hizmet == null)
            {
                return BadRequest(new { message = "Seçilen hizmet bulunamadı." });
            }

            randevu.Sure = hizmet.HizmetSure;
            randevu.Ucret = hizmet.HizmetFiyat;

            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            var yeniRandevu = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Calisan)
                .Where(r => r.RandevuId == randevu.RandevuId)
                .Select(r => new
                {
                    r.RandevuTarih,
                    r.Hizmet.HizmetAd,
                    r.Calisan.CalisanAd,
                    r.Sure,
                    r.Ucret
                })
                .FirstOrDefault();

            return Json(yeniRandevu);
        }



        // AJAX: Hizmet bilgilerini getir (GET)
        [HttpGet]
        public IActionResult HizmetBilgiGetir(int hizmetId)
        {
            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.HizmetId == hizmetId);
            if (hizmet == null)
            {
                return NotFound(new { message = "Hizmet bulunamadı." });
            }

            return Json(new
            {
                sure = hizmet.HizmetSure,
                ucret = hizmet.HizmetFiyat
            });
        }

    }
}
