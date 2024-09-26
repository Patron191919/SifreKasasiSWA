using System.ComponentModel.DataAnnotations;

namespace Uygulama.Models
{
    public class PasswordEntry
    {
        [Key]
        public int Id { get; set; }
        
        public string? SiteAdi { get; set; }
        
        public string? KullaniciAdi { get; set; }
        
        public string? Sifre { get; set; }
    }
}
