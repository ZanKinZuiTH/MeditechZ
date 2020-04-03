using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShareLibrary
{
    public class SymmetricEncryption
    {
        private byte[] Key;
        private byte[] Vector;
        public string Password
        {
            set
            {
                this.GenerateKey(value);
            }
        }
        public SymmetricEncryption()
        {

        }
        public SymmetricEncryption(string password)
        {
            this.GenerateKey(password);
        }
        private void GenerateKey(string password)
        {
            SHA384Managed sHA384Managed = new SHA384Managed();
            byte[] sourceArray = sHA384Managed.ComputeHash(new ASCIIEncoding().GetBytes(password));
            this.Key = new byte[32];
            this.Vector = new byte[16];
            Array.Copy(sourceArray, 0, this.Key, 0, 32);
            Array.Copy(sourceArray, 32, this.Vector, 0, 16);
        }
        public string Encrypt(string plainText, string password)
        {
            this.GenerateKey(password);
            return this.Encrypt(plainText);
        }
        public string Encrypt(string plainText)
        {
            if (this.Key == null)
            {
                throw new InvalidOperationException("Password must be provided or set.");
            }
            byte[] bytes = new ASCIIEncoding().GetBytes(plainText);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ICryptoTransform transform = rijndaelManaged.CreateEncryptor(this.Key, this.Vector);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            cryptoStream.Close();
            memoryStream.Close();
            return Convert.ToBase64String(memoryStream.ToArray());
        }
        public string Decrypt(string encryptedText, string password)
        {
            this.GenerateKey(password);
            return this.Decrypt(encryptedText);
        }
        public string Decrypt(string encryptedText)
        {
            if (this.Key == null)
            {
                throw new InvalidOperationException("Password must be provided or set.");
            }
            byte[] array = Convert.FromBase64String(encryptedText);
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            ICryptoTransform transform = rijndaelManaged.CreateDecryptor(this.Key, this.Vector);
            MemoryStream memoryStream = new MemoryStream(array);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Read);
            byte[] array2 = new byte[array.Length];
            int count = cryptoStream.Read(array2, 0, array2.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return new ASCIIEncoding().GetString(array2, 0, count);
        }
    }
}
