// BLL/SiteService.cs
using Uygulama.DAL;
using Uygulama.Models;
using Uygulama.Helper;
using System.Collections.Generic;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Uygulama.BLL
{
    public class SiteService
    {
           
        private readonly SiteBilgileriRepository _siteBilgileriRepository;
        private readonly KullaniciRepository _kullaniciRepository;

        public SiteService(SiteBilgileriRepository siteBilgileriRepository,KullaniciRepository kullaniciRepository)
        {
            _siteBilgileriRepository = siteBilgileriRepository;
            _kullaniciRepository = kullaniciRepository;
        }
          public Kullanici? GetKullaniciByKullaniciAdi(string kullaniciAdi)
        {
            return _kullaniciRepository.GetKullaniciByKullaniciAdi(kullaniciAdi);
        }
      public List<SiteBilgileri> GetSitesByKullaniciId(int kullaniciId)
{
    var siteListesi = _siteBilgileriRepository.GetSitelerByKullaniciId(kullaniciId);

    
    foreach (var site in siteListesi)
    {
        if (!string.IsNullOrEmpty(site.SiteSifre))
        {
            site.SiteSifre = AES.Decrypt(site.SiteSifre);
        }
        else
        {
            site.SiteSifre = "Şifre Yok";
        }
    }

    return siteListesi;
}


        public List<SiteBilgileri> GetSiteler(int kullaniciId)
        {
            return _siteBilgileriRepository.GetSitelerByKullaniciId(kullaniciId);
        }

       
        public int AddSite(SiteBilgileri siteBilgileri)
        {
            
            if (string.IsNullOrEmpty(siteBilgileri.SiteAdi) || siteBilgileri.SiteAdi.Length > 50)
            {
                return 1; 
            }
            if (string.IsNullOrEmpty(siteBilgileri.SiteKullaniciAdi) || siteBilgileri.SiteKullaniciAdi.Length > 30)
            {
                return 2; 
            }
              if (string.IsNullOrEmpty(siteBilgileri.SiteSifre) || siteBilgileri.SiteSifre.Length > 30)
            {
                return 3; 
            }

            siteBilgileri.SiteSifre=AES.Encrypt(siteBilgileri.SiteSifre);
            _siteBilgileriRepository.AddSite(siteBilgileri);
            return 0;
        }

        public int UpdateSite(SiteBilgileri site)
        {
            if(string.IsNullOrEmpty(site.SiteAdi) || site.SiteAdi.Length > 50){
            return 1;

            }
            if(string.IsNullOrEmpty(site.SiteKullaniciAdi) || site.SiteKullaniciAdi.Length > 30){
            return 2;

            }
            if(string.IsNullOrEmpty(site.SiteSifre) || site.SiteSifre.Length > 30){
            return 3;

            }
            site.SiteSifre=AES.Encrypt(site.SiteSifre);
            _siteBilgileriRepository.UpdateSite(site);
            return 0;
        }

     
   public SiteBilgileri? GetSiteById(int id)
{
    var site = _siteBilgileriRepository.GetSiteById(id);

    if (site == null)
    {
        return null; 
    }

    if (!string.IsNullOrEmpty(site.SiteSifre))
    {
        site.SiteSifre = AES.Decrypt(site.SiteSifre);
    }
    else
    {
        site.SiteSifre = "Şifre Yok";
    }

    return site;
}



public void DeleteSite(SiteBilgileri site)
{
    _siteBilgileriRepository.DeleteSite(site);
}



    }
}
