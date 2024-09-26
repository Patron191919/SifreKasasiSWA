using Uygulama.DAL;
using Uygulama.Models;
using System.Security.Cryptography;
using System.Text;

namespace Uygulama.Helper{
    public class AES{
        public static string Encrypt(string plainText)
{
    using (Aes aesAlg = Aes.Create())
    {
      aesAlg.Key = Convert.FromBase64String("RUU0NkM4REYyNzhDRDU5MzEwNjlCNTIyRTY5NUQ0RjI=");
        aesAlg.IV = Convert.FromBase64String("QTFCMkMzRDRFNUY2MDcwOA==");
        aesAlg.Padding = PaddingMode.PKCS7;  
        aesAlg.Mode = CipherMode.CBC;  

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
            return Convert.ToBase64String(msEncrypt.ToArray());
        }
    }
}
    public static bool IsBase64String(string base64String)
{
    Span<byte> buffer = new Span<byte>(new byte[base64String.Length]);
    return Convert.TryFromBase64String(base64String, buffer, out _);
}
 public static string Decrypt(string cipherText)
{
    if (string.IsNullOrEmpty(cipherText) || !IsBase64String(cipherText))
    {
        return string.Empty; 
    }

    using (Aes aesAlg = Aes.Create())
    {
        aesAlg.Key = Convert.FromBase64String("RUU0NkM4REYyNzhDRDU5MzEwNjlCNTIyRTY5NUQ0RjI=");
        aesAlg.IV = Convert.FromBase64String("QTFCMkMzRDRFNUY2MDcwOA==");
        aesAlg.Padding = PaddingMode.PKCS7;  
        aesAlg.Mode = CipherMode.CBC;        

        try
        {
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
        catch (CryptographicException ex)
        {
            Console.WriteLine($"Şifre çözme hatası: {ex.Message}");
            return "Şifre çözme hatası"; 
        }
    }
}
    }
}