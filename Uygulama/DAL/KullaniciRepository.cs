// DAL/KullaniciRepository.cs
using Uygulama.Models;
using Uygulama.Data;
using System.Linq;

namespace Uygulama.DAL
{
    public class KullaniciRepository
    {
        private readonly ApplicationDbContext _context;

        public KullaniciRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Kullanici? GetKullaniciByKullaniciAdi(string kullaniciAdi)
        {
            return _context.Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi);
        }

        public void AddKullanici(Kullanici kullanici)
        {
            _context.Kullanicilar.Add(kullanici);
            _context.SaveChanges();
        }
        
public Kullanici? GetKullaniciByCredentials(string kullaniciAdi, string hashedPassword)
{
    return _context.Kullanicilar.FirstOrDefault(u => u.KullaniciAdi == kullaniciAdi && u.Sifre == hashedPassword);
}

    }
}
