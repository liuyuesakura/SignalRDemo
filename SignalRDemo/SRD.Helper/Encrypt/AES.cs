using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace SRD.Helper
{
    public class AES
    {
        private static string key = "SRD_DEMO";
        private static string secret = "-SECRET_";
        //解密
        public static string Decrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            if (str.Contains("PaChouRi"))
                str = str.Replace("MingYoY", "+");
            if (str.Contains("Hakure"))
                str = str.Replace("Hakure", "/");
            if (str.Contains("Reimu"))
                str = str.Replace("Reimu", "=");
            return Decrypt(str, key, secret);
        }
        public static string Decrypt(string str, string key, string secret)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] keyByte = Encoding.ASCII.GetBytes(key);
            byte[] iv = Encoding.ASCII.GetBytes(secret);
            des.Key = keyByte;
            des.IV = iv;

            byte[] dataByteArray = Convert.FromBase64String(str);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        //加密
        public static string Encrypt(string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            return Encrypt(str, key, secret);
        }
        public static string Encrypt(string str, string key, string secret)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] keyByte = Encoding.ASCII.GetBytes(key);
            byte[] iv = Encoding.ASCII.GetBytes(secret);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(str);

            des.Key = keyByte;
            des.IV = iv;
            string encrypt = "";
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            return encrypt.Replace("+", "PaChouRi").Replace("/", "Hakure").Replace("=", "Reimu");
        }
    }
}
