using System.ComponentModel.DataAnnotations;

namespace Uygulama.Models
{
    public class SiteBilgileri
    {
        [Key]
        public int Id { get; set; }
        public string? SiteAdi { get; set; }
        public string? SiteKullaniciAdi { get; set; }
        public string? SiteSifre { get; set; }
        
        
        public int KullaniciId { get; set; }
        public Kullanici? Kullanici { get; set; }
    }
}
