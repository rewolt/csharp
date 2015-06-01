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
            // ZAŁOŻENIA: Program ma zmieniać nazwy katalogów. Zmiana nazwy katalogu wiąże się ze zmianą ścieżki dostępu
            // do katalogu weenątrz zmienianego katalogu. Wymusza to rozpoczęcie zmieniania nazw od katalogów najbardziej
            // zagnieżdżonych

            Console.WriteLine("||  Skrót nazw katalogów i plików w bazie DICOM by ADMIN  ||");
            Console.WriteLine("||  - - - - - - - - - - - - - - - - - - - - - - - - - -   ||\n");

            // Zamknięcie na wypadek pustego przekazania do programu
            if (args.Length == 0 || (System.IO.File.Exists(args[0])))
            {               
                Console.WriteLine("Przeciągnij katalog bazy do posprzątania na ikonkę programu :)");
                Console.WriteLine("Naciśnij cokolwiek, by zamknąć . . .");
                Console.ReadKey();
                Environment.Exit(0);
            }
           
            // Tworzenie listy na pliki i katoalogi
            List<string> foldery = new List<string>(args);
            List<string> pliki = new List<string>();

            //----------------------------------------------------------------
            // Odnajdywanie ścieżek do plików i katalogów
            Console.WriteLine("Przeszukiwanie folderu...");
            for (int i = 0; i < foldery.Count; i++)
            {
                foldery.AddRange(System.IO.Directory.GetDirectories(foldery[i]));
                pliki.AddRange(System.IO.Directory.GetFiles(foldery[i]));
            }

            //---------------------------------------------------------------
            // Zmiana nazw plików na krótsze - OPERACJA NA PLIKACH
            foreach (string plik in pliki)
                if (plik.Contains("OBRAZ"))
                    System.IO.File.Move(plik, plik.Replace("OBRAZ", "IM"));
            


            // ---------------------------------------------------------------
            // Zmiana nazw katalogów na krótsze

            // Tworzenie list na ścieżki do folderów; sort czyli posortowane, gotowe czyli przetworzone
            List<string> folderySort = new List<string>();
            List<string> folderyGotowe = new List<string>();
            
            // Sortowanie od długości ścieżki dostępu potrzebne, by móc zsynchronizować listy ze
            // ścieżkami źródłowymi i ścieżkami docelowymi.
            // NIEZMIERNIE WAŻNA jest kolejność zmian nazw katalogów. Patrz założenia
            foreach (string s in SortByLength(foldery))
                folderySort.Add(s);

            // Tworzenie nowych ścieżek dostępu. Z każdej ścieżki jest brana nazwa ostatniego katalogu
            // Nazwa zostaje skrócona do 2 liter + iterator, by nie było 2ch katalogów o tej samej nazwie.
            string temp="";
            int iteracja= 1;
            foreach (string sciezka in folderySort)
            {
                string[] folder = sciezka.Split('\\');
                if (folder[folder.Length - 1].Length >= 3)
                {
                    folder[folder.Length - 1] = folder[folder.Length - 1].Remove(2);
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
                
            // Zapisanie nowej struktury katalogów i plików do txt
            StreamWriter sr = new StreamWriter(args[0] + "_zestawienie.txt");
            sr.WriteLine("ZESTAWIENIE - znaczenie mają tylko nazwy ostatnich katalogów w każdej ścieżce dostępu");
            sr.WriteLine("\n====== KATALOGI =======");
            folderyGotowe.ForEach(delegate(String folder) { sr.WriteLine(folder); });
            sr.Flush(); sr.Close();
            Console.WriteLine("Zapisano zrzut nowego drzewa katalogów do pliku");

            // Takie tam zabezpieczenie, na wypadek gdyby lista źródłowa i lista docelowa jednak nie były sobie równe...
            if (folderySort.Count != folderyGotowe.Count)
                Console.WriteLine("Nierówna ilość ścieżek! Sprawdź foldery, bo coś się skopało");
            else
            {
                // Dla każdego katalogu ze ściezki źródłowej zmień jego nazwę na taką jak w ścieżce docelowej.
                for (int piwo = 0; piwo < folderyGotowe.Count; piwo++)
                {
                    // Zmień nazwę tylko gdy mają różne nazwy - inaczej nazwa już jest krótka, nie ma co zmieniać
                    if (folderySort[piwo]!=folderyGotowe[piwo])
                        // Zmiana nazwy katalogu - OPERACJE NA KATALOGACH
                        System.IO.Directory.Move(folderySort[piwo], folderyGotowe[piwo]);
                }
            }

            Console.WriteLine("Zmiana katalogów zakończyła się sukcesem!\nNaciśnij cokolwiek, by zamknąć . . .");
            Console.ReadKey();
        }





        // Sortowanie jakiejkolwiek listy od najdłuższej ścieżki do najkrótszej
        static IEnumerable<string> SortByLength(IEnumerable<string> e)
        {
            // LINQ zwraca posortowaną kopię wsadzonej listy...
            var sorted = from s in e
                         orderby s.Length descending
                         select s;
            return sorted;
        }
    }
}
