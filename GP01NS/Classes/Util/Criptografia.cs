using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GP01NS.Classes.Util
{
    public static class Criptografia
    {
        private static readonly TripleDESCryptoServiceProvider TripDES = new TripleDESCryptoServiceProvider();
        private static readonly UTF8Encoding UTFEncod = new UTF8Encoding();
        private static readonly byte[] Keys = { 69, 116, 105, 101, 110, 110, 101, 32, 65, 108, 118, 101, 115, 32, 100, 111, 115, 32, 83, 97, 110, 116, 111, 115 };
        private static readonly byte[] IV = { 68, 97, 114, 107, 115, 111, 117, 108 };

        public static string Criptografar(string s) 
        {
            byte[] input = UTFEncod.GetBytes(s);
            byte[] output = Transformar(input, TripDES.CreateEncryptor(Keys, IV));

            return Convert.ToBase64String(output);
        }

        public static string Descriptografar(string s) 
        {
            byte[] input = Convert.FromBase64String(s);
            byte[] output = Transformar(input, TripDES.CreateDecryptor(Keys, IV));

            return UTFEncod.GetString(output);
        }

        public static string GetHash64(string s) 
        {
            return ToSHA256(GerarSHA256(s));
        }

        public static string GetHash64(string s, DateTime data) 
        {
            return ToSHA256(GerarSHA256(s, data));
        }

        public static string GetHash128(string s) 
        {
            return ToSHA512(GerarSHA512(s));
        }

        public static string GetHash128(string s, DateTime data) 
        {
            return ToSHA512(GerarSHA512(s, data));
        }

        public static bool ValidarHash64(string s, string hash)
        {
            var sha = ToSHA256(GerarSHA256(s));

            if (hash == sha)
                return true;
            else
                return false;
        }

        public static bool ValidarHash64(string s, DateTime data, string hash) 
        {
            var sha = ToSHA256(GerarSHA256(s, data));

            if (hash == sha)
                return true;
            else
                return false;
        }

        public static bool ValidarHash128(string s, string hash)
        {
            var sha = ToSHA512(GerarSHA512(s));

            if (hash == sha)
                return true;
            else
                return false;
        }

        public static bool ValidarHash128(string s, DateTime data, string hash) 
        {
            var sha = ToSHA512(GerarSHA512(s, data));

            if (hash == sha)
                return true;
            else
                return false;
        }

        #region PRIVATES
        private static string GerarSHA256(string s) 
        {
            var shaString = ToSHA256(s + "GP01NS");

            var hash = string.Empty;

            for (int i = 0; i < 64; i++)
            {
                hash += shaString[i].ToString();
            }

            return hash;
        }

        private static string GerarSHA256(string s, DateTime data) 
        {
            var shaString = ToSHA256(s + "GP01NS");
            var shaData = ToSHA256(data.ToString("dd-MM-yyyy hh:mm:ss"));

            var hash = string.Empty;

            for (int i = 0; i < 64; i++)
            {
                hash += shaString[i].ToString() + shaData[i].ToString();
            }

            return hash;
        }

        private static string GerarSHA512(string s) 
        {
            var shaString = ToSHA512(s + "GP01NS");

            var hash = string.Empty;

            for (int i = 0; i < 128; i++)
            {
                hash += shaString[i].ToString();
            }

            return hash;
        }

        private static string GerarSHA512(string s, DateTime data) 
        {
            var shaString = ToSHA512(s + "GP01NS");
            var shaData = ToSHA512(data.ToString("dd-MM-yyyy hh:mm:ss"));

            var hash = string.Empty;

            for (int i = 0; i < 128; i++)
            {
                hash += shaString[i].ToString() + shaData[i].ToString();
            }

            return hash;
        }

        private static string ToSHA256(string s) 
        {
            SHA256 sha = SHA256.Create();

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] hash = sha.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private static string ToSHA512(string s) 
        {
            SHA512 sha = SHA512.Create();

            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
            byte[] hash = sha.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private static byte[] Transformar(byte[] input, ICryptoTransform cryptoTransform) 
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cryptStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptStream.Write(input, 0, input.Length);
                    cryptStream.FlushFinalBlock();

                    ms.Position = 0;
                    byte[] arr = ms.ToArray();

                    ms.Close();
                    cryptStream.Close();

                    return arr;
                }
            }
        }
        #endregion PRIVATES
    }
}