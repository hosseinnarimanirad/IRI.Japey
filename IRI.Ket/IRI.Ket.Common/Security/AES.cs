using System;
using System.Security.Cryptography;
using System.IO;

namespace IRI.Ket.Common.Security
{
    public class AES
    {

        public AES()
        {
        }

        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {

            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();
            alg.Key = Key;

            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(clearData, 0, clearData.Length);
            cs.Close();
            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }


        public string Encrypt(string Data, string Password, int Bits)
        {

            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(Data);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

            if (Bits == 128)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 192)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else if (Bits == 256)
            {
                byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return Convert.ToBase64String(encryptedData);
            }
            else
            {
                return string.Concat(Bits);
            }
        }


        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {

            MemoryStream ms = new MemoryStream();
            Rijndael alg = Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;
            CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }


        public string Decrypt(string Data, string Password, int Bits)
        {

            byte[] cipherBytes = Convert.FromBase64String(Data);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,

                new byte[] { 0x00, 0x01, 0x02, 0x1C, 0x1D, 0x1E, 0x03, 0x04, 0x05, 0x0F, 0x20, 0x21, 0xAD, 0xAF, 0xA4 });

            if (Bits == 128)
            {
                byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(16), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else if (Bits == 192)
            {
                byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(24), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else if (Bits == 256)
            {
                byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            else
            {
                return string.Concat(Bits);
            }

        }

    }
}
