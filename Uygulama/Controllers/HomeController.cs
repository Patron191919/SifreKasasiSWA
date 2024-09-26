using Microsoft.AspNetCore.Mvc;
using Uygulama.BLL;
using Uygulama.Models;
using Microsoft.AspNetCore.Http;
using Uygulama.Data;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;  // RemoteWebDriver için
using OpenQA.Selenium.Support.UI;  // WebDriverWait için
using SeleniumExtras.WaitHelpers;   // ExpectedConditions için
using System;
using OpenQA.Selenium.Chrome;


namespace Uygulama.Controllers
{
    public class HomeController : Controller
    {
        private readonly KullaniciService _kullaniciService;
        private readonly SiteService _siteService;
        private readonly ApplicationDbContext _context;

        public HomeController(KullaniciService kullaniciService, SiteService siteService, ApplicationDbContext context)
        {
            _kullaniciService = kullaniciService;
            _siteService = siteService;
            _context = context;
        }

        public IActionResult Kayıt()
        {
            DisableBrowserCache();
            return View();
        }

        public IActionResult SiteEkle()
        {
            DisableBrowserCache();
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return RedirectToAction("Login");
            }

            var kullanici = _siteService.GetKullaniciByKullaniciAdi(kullaniciAdi);

            if (kullanici == null)
            {
                return RedirectToAction("Login");
            }

            var siteListesi = _siteService.GetSitesByKullaniciId(kullanici.Id);
            return View(siteListesi);
        }

        public IActionResult Index()
        {
            DisableBrowserCache();
            return View();
        }

        public IActionResult Login()
        {
            DisableBrowserCache();
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            DisableBrowserCache();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Register(string Ad, string Soyad, string KullaniciAdi, string Mail, string Sifre)
        {
            if (_kullaniciService.IsKullaniciExists(KullaniciAdi))
            {
                return Json(new { success = false, message = "Bu kullanıcı adı zaten kullanılıyor." });
            }

            var kullanici = new Kullanici
            {
                Ad = Ad,
                Soyad = Soyad,
                KullaniciAdi = KullaniciAdi,
                Mail = Mail,
                Sifre = Sifre
            };

            int sayi = _kullaniciService.RegisterKullanici(kullanici);
            if (sayi == 1)
            {
                return Json(new { success = false, message = "İsim 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 2)
            {
                return Json(new { success = false, message = "Soy İsim 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 3)
            {
                return Json(new { success = false, message = "Kullanıcı Adı 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 4)
            {
                return Json(new { success = false, message = "Mail 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 5)
            {
                return Json(new { success = false, message = "Şifre 30 karakterden fazla ve sadece boşluktan oluşamaz.." });
            }

            HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);

            return Json(new { success = true });
        }

        private void DisableBrowserCache()
        {
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "-1";
        }

        [HttpPost]
        public JsonResult AddSite(string siteAdi, string siteKullaniciAdi, string siteSifre)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return Json(new { success = false, message = "Kullanıcı kimliği bulunamadı." });
            }

            var kullanici = _kullaniciService.GetKullaniciByKullaniciAdi(kullaniciAdi);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var siteBilgisi = new SiteBilgileri
            {
                SiteAdi = siteAdi,
                SiteKullaniciAdi = siteKullaniciAdi,
                SiteSifre = siteSifre,
                KullaniciId = kullanici.Id
            };

            int result = _siteService.AddSite(siteBilgisi);

            if (result == 0)
            {
                return Json(new { success = true });
            }
            if (result == 1)
            {
                return Json(new { success = false, message = "Site Adı 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (result == 2)
            {
                return Json(new { success = false, message = "Kullanıcı Adı 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (result == 3)
            {
                return Json(new { success = false, message = "Şifre 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }

            return Json(new { success = false, message = "mesaj404" });
        }

        [HttpGet]
        public JsonResult GetSiteler()
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return Json(new { success = false, message = "Kullanıcı kimliği bulunamadı." });
            }

            var kullanici = _kullaniciService.GetKullaniciByKullaniciAdi(kullaniciAdi);

            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var siteListesi = _siteService.GetSitesByKullaniciId(kullanici.Id);

            var result = siteListesi.Select(s => new
            {
                id = s.Id,
                siteAdi = s.SiteAdi,
                siteKullaniciAdi = s.SiteKullaniciAdi,
                siteSifre = s.SiteSifre
            }).ToList();

            return Json(result);
        }

        [HttpPost]
        public JsonResult LoginUser(string username, string password)
        {
            var kullanici = _kullaniciService.GetKullaniciByCredentials(username, password);

            if (kullanici?.KullaniciAdi != null)
            {
                HttpContext.Session.SetString("KullaniciAdi", kullanici.KullaniciAdi);
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Kullanıcı adı veya şifre yanlış." });
        }

        [HttpPost]
        public JsonResult DeleteSite(int id)
        {
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi");

            if (string.IsNullOrEmpty(kullaniciAdi))
            {
                return Json(new { success = false, message = "Kullanıcı kimliği bulunamadı." });
            }

            var kullanici = _kullaniciService.GetKullaniciByKullaniciAdi(kullaniciAdi);
            if (kullanici == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            var site = _siteService.GetSiteById(id);
            if (site == null || site.KullaniciId != kullanici.Id)
            {
                return Json(new { success = false, message = "Site bulunamadı veya bu kullanıcıya ait değil." });
            }

            _siteService.DeleteSite(site);
            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetSiteById(int id)
        {
            var site = _siteService.GetSiteById(id);

            if (site == null)
            {
                return Json(new { success = false, message = "Site bulunamadı." });
            }

            return Json(new
            {
                id = site.Id,
                siteAdi = site.SiteAdi,
                siteKullaniciAdi = site.SiteKullaniciAdi,
                siteSifre = site.SiteSifre
            });
        }

        [HttpPost]
        public JsonResult UpdateSite(int id, string siteAdi, string siteKullaniciAdi, string siteSifre)
        {
            var site = _siteService.GetSiteById(id);

            if (site == null)
            {
                return Json(new { success = false, message = "Site bulunamadı." });
            }

            site.SiteAdi = siteAdi;
            site.SiteKullaniciAdi = siteKullaniciAdi;
            site.SiteSifre = siteSifre;

            int sayi = _siteService.UpdateSite(site);
            if (sayi == 0)
            {
                return Json(new { success = true, message = "Site başarıyla güncellendi." });
            }
            if (sayi == 1)
            {
                return Json(new { success = false, message = "Site Adı 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 2)
            {
                return Json(new { success = false, message = "Kullanıcı Adı 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }
            if (sayi == 3)
            {
                return Json(new { success = false, message = "Sifre 30 karakterden fazla ve sadece boşluktan oluşamaz." });
            }

            return Json(new { success = false, message = "Mesaj404" });
        }

       [HttpPost]
public JsonResult AutoLogin(string siteAdi, string siteKullaniciAdi, string siteSifre)
{
    try
    {
        // Kullanıcıya başarılı yanıt dön
        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}




    }
}
