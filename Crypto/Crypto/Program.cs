using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace Crypto
{
    class Program
    {
        // Nagłówek
        static void Naglowek ()
        {
            Console.WriteLine("||----------------------------------------------------------||");
            Console.WriteLine("||                  Crypto v1.0 by Admin                    ||");
            Console.WriteLine("||----------------------------------------------------------||");
            Console.WriteLine();
        }

        // Szyfrowanie mechanizm
        static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Flush();
                        cs.Dispose();
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        // Deszyfrowanie mechanizm
        static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;
                    

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Flush();
                        cs.Dispose();
                        cs.Close();
                    }

                    AES.Clear();
                    AES.Dispose();
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        // Szyfrowanie pliku
        static void SzyfrujPlik(string file, string password)
        {
            //string file = "C:\\SampleFile.DLL";
            //string password = "abcd1234";

            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);


            string fileEncrypted = file+".szyfrowany";

            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
            
        }

        // Deszyfrowanie pliku
        static void DeszyfrujPlik(string fileEncrypted, string password)
        {
            //string fileEncrypted = "C:\\SampleFileEncrypted.DLL";
            //string password = "abcd1234";

            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string file = fileEncrypted.Remove(fileEncrypted.LastIndexOf("."));
            File.WriteAllBytes(file, bytesDecrypted);
        }

        // Main z wyborem funkcji
        static void Main(string[] args)
        {
            string password;

            Naglowek();

            if (args.Length == 0)
            {
                Console.WriteLine("Brak pliku!\nPrzeciągnij plik na ikonkę programu, by uruchomić szyfrowanie.");
                Console.WriteLine("Naciśnij cokolwiek, by zakończyć. . .");
            }

            else
            {
                Console.WriteLine("Wybierz:\n1 - Szyfrowanie\n2 - Deszyfrowanie");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        Console.Write("Hasło: ");
                        password = Console.ReadLine();
                        SzyfrujPlik(args[0], password);
                        Console.WriteLine("Szyfrowanie zakończone. Plik zapisano:\n{0}.szyfrowany", args[0]);
                        break;
                    case 2:
                        Console.Write("Hasło: ");
                        password = Console.ReadLine();
                        DeszyfrujPlik(args[0], password);
                        Console.WriteLine("Deszyfrowanie zakończone. Plik zapisano:\n{0}", args[0].Remove(args[0].LastIndexOf(".")));
                        break;
                    default:
                        Console.WriteLine("Niewłaściwy wybór.\nNaciśnij dowolny klawisz, by zakończyć. . .");
                        break;
                }
            }
            Console.ReadKey();
        }
    }
}
