using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace md5_sha1_generator
{
    class Program
    {
        public static MD5 md5 = MD5.Create();
        public static SHA1 sha1 = SHA1.Create();

        static void Naglowek ()
        {
            Console.WriteLine("|--------------------------------------------------------------------|");
            Console.WriteLine("|                   MD5 & SHA-1 generator by Admin                   |");
            Console.WriteLine("|--------------------------------------------------------------------|\n");            
        }

        static byte[] md5GenAsync(string lokalizacja)
        {           
            var plik = File.OpenRead(lokalizacja);
            var computed = md5.ComputeHash(plik);
            return computed;            
        }

        static byte[] sha1Gen(string lokalizacja)
        {
            var plik = File.OpenRead(lokalizacja);
            var computed = sha1.ComputeHash(plik);
            return computed;
        }
        static void Main(string[] args)
        {
            
            Naglowek();
            if (args.Length != 0)
            {
                int i = 1;
                foreach (string lokalizacja in args)
                {                   
                    Console.WriteLine("Obliczanie pliku: {0} z {1} ({2})", i, args.Length, args[i-1]);
                    Console.Write("MD5:");
                    Console.Write("\t{0}\n", BitConverter.ToString(md5GenAsync(lokalizacja)).Replace("-", "").ToLower());
                    Console.Write("SHA-1:");
                    Console.Write("\t{0}\n", BitConverter.ToString(sha1Gen(lokalizacja)).Replace("-", "").ToLower());
                    Console.WriteLine();
                    i++;
                }
                Console.WriteLine("Koniec obliczeń! :)\nNaciśnij cokolwiek, by zakończyć . . .");
            }
            else
                Console.WriteLine("Brak pliku! Przeciągnij pliki na ikonkę programu.\nNaciśnij cokolwiek, by zakończyć. . .");
            Console.ReadKey();
            
        }
    }
}
