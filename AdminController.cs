using Kuafor1.Models; // Veritabanı modelleri için
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
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
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre)
        {
            string adminKullaniciAdi = "B201210017@sakarya.edu.tr";
            string adminSifre = "sau";

            if (kullaniciAdi == adminKullaniciAdi && sifre == adminSifre)
            {
                // Kullanıcı bilgileri oluşturulur
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, kullaniciAdi),
                    new Claim(ClaimTypes.Role, "Admin") // Admin rolü atanır
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Giriş işlemi yapılır
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Islemler");
            }
            else
            {
                ViewBag.Mesaj = "Kullanıcı adı veya şifre hatalı.";
            }

            return View();
        }

        [Authorize]
        public IActionResult Islemler()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult >Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Giris");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult HizmetIslemleri()
        {
            var hizmetler = _context.Hizmetler.ToList(); // DB'den hizmetleri çekiyoruz
            return View(hizmetler); // Görünüme hizmet listesini gönderiyoruz
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult CalisanIslemleri()
        {
            var calisanlar = _context.Calisanlar
                .Include(c => c.CalisanUzmanliklar) // Uzmanlık alanlarını da dahil et
                .ToList();
            var hizmetler = _context.Hizmetler.ToList(); // Hizmetleri al
            ViewBag.Hizmetler = hizmetler; // Görünüme hizmetleri gönder
            return View(calisanlar);
        }

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
        public IActionResult UyeListele()
        {
            var uyeler = _context.Uyeler.ToList(); // Veritabanından tüm üyeleri al
            return View(uyeler); // Görünüme gönder
        }

        [Authorize(Roles = "Admin")]
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
