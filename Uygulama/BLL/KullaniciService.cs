// BLL/KullaniciService.cs
using Uygulama.DAL;
using Uygulama.Models;
using Uygulama.Helper;
using System.Security.Cryptography;
using System.Text;

namespace Uygulama.BLL
{
    public class KullaniciService
    {
        private readonly KullaniciRepository _kullaniciRepository;

        public KullaniciService(KullaniciRepository kullaniciRepository)
        {
            _kullaniciRepository = kullaniciRepository;
        }

        public bool IsKullaniciExists(string kullaniciAdi)
        {
            return _kullaniciRepository.GetKullaniciByKullaniciAdi(kullaniciAdi) != null;
        }

        public int RegisterKullanici(Kullanici kullanici)
        {
            if(string.IsNullOrEmpty(kullanici.Ad) || kullanici.Ad.Length > 30){
            return 1;

            }
            if(string.IsNullOrEmpty(kullanici.Soyad) || kullanici.Soyad.Length > 30){
            return 2;

            }
            if(string.IsNullOrEmpty(kullanici.KullaniciAdi) || kullanici.KullaniciAdi.Length > 30){
            return 3;

            }
            if(string.IsNullOrEmpty(kullanici.Mail) || kullanici.Mail.Length > 30){
            return 4;

            }
            if(string.IsNullOrEmpty(kullanici.Sifre) || kullanici.Sifre.Length > 30){
            return 5;

            }

            
            kullanici.Sifre = Hash.ComputeSha256Hash(kullanici.Sifre);
            _kullaniciRepository.AddKullanici(kullanici);
            return 0;
            

        }

       
        // BLL/KullaniciService.cs
public Kullanici? GetKullaniciByCredentials(string kullaniciAdi, string Password)
{
    var HashPassword= Hash.ComputeSha256Hash(Password);
    return _kullaniciRepository.GetKullaniciByCredentials(kullaniciAdi, HashPassword);
}

         public Kullanici? GetKullaniciByKullaniciAdi(string kullaniciAdi)
        {
            
            return _kullaniciRepository.GetKullaniciByKullaniciAdi(kullaniciAdi);
        }
    }
}
