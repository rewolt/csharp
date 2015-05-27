using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Zmiana_nazw_plików_i_katalogów
{
    class Program
    {
        static void Main(string[] args)
        {


            //string[] args1 = { "D:\\ZSI\\kolos1\\zad2 - Kopia" };


            // Zamknięcie na wypadek pustego przekazania
            if (args.Length == 0 || (System.IO.File.Exists(args[0])))
            {
                Console.WriteLine("Przenieś katalog na plik programu, by posprzątać podkatalogi i nazwy plików.");
                Console.ReadKey();
                Environment.Exit(0);
            }


            // Tworzenie listy plików i katalogów
            List<string> sciezkiFolderow = new List<string>(args);
            List<string> sciezkiPlikow = new List<string>();

            // Odnajdywanie plików i katalogów
            for (int i = 0; i < sciezkiFolderow.Count; i++)
            {
                sciezkiFolderow.AddRange(System.IO.Directory.GetDirectories(sciezkiFolderow[i]));
                sciezkiPlikow.AddRange(System.IO.Directory.GetFiles(sciezkiFolderow[i]));
            }


            // Zmiana nazw plików na krótsze
            for (int i = 0; i < sciezkiPlikow.Count; i++)
            {                
                string nowaSciezka = sciezkiPlikow[i].Remove(sciezkiPlikow[i].LastIndexOf('\\')+1);
                nowaSciezka+="IM"+i.ToString();
                if (sciezkiPlikow[i]!=nowaSciezka)
                    System.IO.File.Move(sciezkiPlikow[i], nowaSciezka);
            }

            List<string> folderySort = new List<string>();
            List<string> folderyGotowe = new List<string>();
            // Zmiana nazw katalogów na krótsze
            foreach (string s in SortByLength(sciezkiFolderow))
                folderySort.Add(s);

            string temp="";
            int iteracja= 1;
            foreach (string sciezka in folderySort)
            {
                
                // Cięcie ściezki folderu na nazwy podfolderów
                string[] folder = sciezka.Split('\\');
                // Jeżeli długość nazwy ostatniego folderu jest dłuższa od 3 znaków
                if (folder[folder.Length - 1].Length >= 3)
                {
                    // Usuń te nadmiarowe znaki zaczynając od 3-ciego
                    folder[folder.Length - 1] = folder[folder.Length - 1].Remove(2);
                    // Jesli przypadkiem nazwa folderu jest 
                    if (temp != folder[folder.Length - 1])
                    {
                        temp = folder[folder.Length - 1];
                        iteracja = 1;
                    }
                    else
                        iteracja++;
                    folder[folder.Length - 1] = folder[folder.Length - 1] + iteracja;
                }

                string sciezkaCala = "";
                foreach (string s in folder)
                {
                    sciezkaCala = sciezkaCala + s + "\\";
                }
                sciezkaCala = sciezkaCala.Remove(sciezkaCala.Length - 1);
                folderyGotowe.Add(sciezkaCala);
            }
                  
            StreamWriter sr = new StreamWriter(args[0] + "zestawienie.txt");
            folderyGotowe.ForEach(delegate(String folder) { sr.WriteLine(folder); });
            //pliki.ForEach(delegate(String plik) { sr.WriteLine(plik); });
            sr.Flush();
            sr.Close();
            Console.WriteLine("Zapisano zrzut drzewa katalogów");

            if (folderySort.Count != folderyGotowe.Count)
            {
                Console.WriteLine("Nierówna ilość elementów! Sprawdź foldery");
            }
            else
            {
                for (int ale = 0; ale < folderyGotowe.Count; ale++)
                {
                    if (folderySort[ale]!=folderyGotowe[ale])
                        System.IO.Directory.Move(folderySort[ale], folderyGotowe[ale]);
                    //Console.WriteLine(ale);
                }
            }

            Console.WriteLine("Naciśnij cokolwiek by zakończyć . . .");
            Console.ReadKey();
        }

        // Sortowanie listy od najdłuższej ścieżki do najkrótszej
        static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // Uzycie LINQ do zwrócenia posortowanej tablicy
            var sorted = from s in e
                         orderby s.Length descending
                         select s;
            return sorted;
        }
    }
}
