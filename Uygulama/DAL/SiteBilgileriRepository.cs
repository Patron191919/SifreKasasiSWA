// DAL/SiteBilgileriRepository.cs
using Uygulama.Models;
using Uygulama.Data;
using System.Collections.Generic;
using System.Linq;

namespace Uygulama.DAL
{
    public class SiteBilgileriRepository
    {
        private readonly ApplicationDbContext _context;

        public SiteBilgileriRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<SiteBilgileri> GetSitelerByKullaniciId(int kullaniciId)
        {
            return _context.SiteBilgileri.Where(s => s.KullaniciId == kullaniciId).ToList();
        }

        public void AddSite(SiteBilgileri site)
        {
            _context.SiteBilgileri.Add(site);
            _context.SaveChanges();
        }

        public void UpdateSite(SiteBilgileri site)
        {
            _context.SiteBilgileri.Update(site);
            _context.SaveChanges();
        }

        public void DeleteSite(SiteBilgileri site)
        {
            _context.SiteBilgileri.Remove(site);
            _context.SaveChanges();
        }
        
public SiteBilgileri? GetSiteById(int id)
{
    return _context.SiteBilgileri.FirstOrDefault(s => s.Id == id);
}

    }
}
