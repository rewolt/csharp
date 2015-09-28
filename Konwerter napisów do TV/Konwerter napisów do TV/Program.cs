using System;
using System.Text;
using System.IO;

namespace Konwerter_napisow
{

    struct CzasT
    {
        public TimeSpan t1, t2;
        public CzasT(TimeSpan t1, TimeSpan t2)
        {
            this.t1 = t1;
            this.t2 = t2;
        }

        public TimeSpan GetT1()
        {
            return t1;
        }

        public TimeSpan GetT2()
        {
            return t2;
        }
    }

    class Program
    {
        // Do wpisywania czasu w plik nowych napisów - dla milisekund 
        static string DodanieZera(int liczba)
        {
            string ret = null;
            if (liczba < 10)
            {
                ret = "0" + liczba.ToString();

            }
            else
                ret = liczba.ToString();
            return ret;
        }

        // Do wpisywania czasu w plik nowych napisów - dla milisekund
        static string DodanieDrugiegoZera(int liczba)
        {
            string ret = null;
            if (liczba < 100)
            {
                ret = "0" + liczba.ToString();
                if (liczba < 10)
                    ret = "0" + ret;
            }
            else
                ret = liczba.ToString();
            return ret;
        }

        //Podmiana znakow
        static string PodmianaZnakow(string wszystko)
        {
            wszystko = wszystko.Replace("ą", "a");
            wszystko = wszystko.Replace("ć", "c");
            wszystko = wszystko.Replace("ę", "e");
            wszystko = wszystko.Replace("ł", "l");
            wszystko = wszystko.Replace("ń", "n");
            wszystko = wszystko.Replace("ó", "o");
            wszystko = wszystko.Replace("ś", "s");
            wszystko = wszystko.Replace("ź", "z");
            wszystko = wszystko.Replace("ż", "z");

            wszystko = wszystko.Replace("Ą", "A");
            wszystko = wszystko.Replace("Ć", "C");
            wszystko = wszystko.Replace("Ę", "E");
            wszystko = wszystko.Replace("Ł", "L");
            wszystko = wszystko.Replace("Ń", "N");
            wszystko = wszystko.Replace("Ó", "O");
            wszystko = wszystko.Replace("Ś", "S");
            wszystko = wszystko.Replace("Ź", "Z");
            wszystko = wszystko.Replace("Ż", "Z");

            wszystko = wszystko.Replace("/", "");
            wszystko = wszystko.Replace("\\", "");

            wszystko = wszystko.Replace("|", "\r\n");

            return wszystko;
        }

        // Wywalenie znaczników klatek
        static string SamTekst(string wszystko)
        {
            if (RozpoznajFormat(wszystko) == 0)
            {
                wszystko = PodmianaZnakow(wszystko);
                wszystko = wszystko.Remove(0, wszystko.LastIndexOf(']') + 1);
            }

            if (RozpoznajFormat(wszystko) == 1)
            {
                wszystko = PodmianaZnakow(wszystko);
                wszystko = wszystko.Remove(0, wszystko.LastIndexOf('}') + 1);
            }

            if (RozpoznajFormat(wszystko) == 2)
            {
                wszystko = PodmianaZnakow(wszystko);
                wszystko = wszystko.Remove(0, wszystko.LastIndexOf(':') + 1);
            }
            return wszystko;
        }

        // Przeróbka klatek na czas
        static TimeSpan ZnacznikCzasu(double start, double klatki)
        {
            // Przeróbka klatek na czas.
            start = start / klatki;
            string starts = start.ToString();
            int poNic;

            // Dodawanie części ułamkowej do stringa jeśli nie posiada
            if (int.TryParse(starts, out poNic))
                starts = starts + ",0";

            //Usunięcie części całkowitej
            starts = starts.Remove(0, starts.LastIndexOf(',') + 1);

            if (starts.Length > 3)             // Sprawdzenie długości części ułamkowej, ograniczenie do 3 miejsc po przecinku
                starts = starts.Remove(3);
            if (starts.Length == 2)
                starts = starts + "0";      // Doklejenie brakujących zer z tyłu do tych 3 miejsc
            if (starts.Length == 1)
                starts = starts + "00";
            if (int.Parse(starts) != 0)
            {
                starts = starts.TrimStart('0');
                if (starts == null)
                    starts = "0";
            }

            int milisek1 = int.Parse(starts.ToString());    // Całkowita ilość milisekund 
            int sek1cal = (int)start;                   // Całkowita ilość sekund, rzutowanie na int usuwa ułamek
            int godz1c = (int)sek1cal / 3600;   // Godziny - ile?
            int godz1r = sek1cal % 3600;      // Pozozstałe sekundy do obrobienia
            int min1cal = (int)godz1r / 60;    // Minuty - ile?
            int min1r = (int)godz1r % 60;      // Pozostałe sekundy doobrobienia

            TimeSpan tsStart = new TimeSpan(0, godz1c, min1cal, min1r, milisek1);
            return tsStart;
        }

        // Rozpoznanie formatu:
        // Decymalny [,] = 0 ;
        // Wymagający framerate {,} = 1;
        // Format czau uproszczonego hh:mm:ss = 2
        // Nieznany = 3
        static int RozpoznajFormat(string tekst)
        {
            if (tekst.StartsWith("["))
                return 0;
            else if (tekst.StartsWith("{"))
                return 1;
            else
            {
                string[] czas = tekst.Split(':');
                int poNic;

                if (int.TryParse(czas[0], out poNic))
                    return 2;
                else
                    return 3; // Błąd, nierozpoznany format
            }
        }

        // Wybór klatek na sekundę dla formatu tego wymagającego
        static double Framerate()
        {
            Console.WriteLine("ROZPOZNANY FORMAT WYMAGA PODANIA FRAMERATE'U\nWybierz framerate (ilość klatek na sekundę) filmu:");
            Console.WriteLine("1. 23,976 klatek/s (domyślnie)");
            Console.WriteLine("2. 29,970 klatek/s");
            Console.WriteLine("3. 25 klatek/s");
            Console.WriteLine("4. 30 klatek/s");
            Console.WriteLine("5. Inna");
            int wybor = int.Parse(Console.ReadLine());
            double klatki;

            switch (wybor)
            {
                case 1:
                    klatki = 23.976;
                    break;
                case 2:
                    klatki = 29.970;
                    break;
                case 3:
                    klatki = 25;
                    break;
                case 4:
                    klatki = 30;
                    break;
                default:
                    Console.Clear();
                    WypiszNaglowek();
                    Console.WriteLine("Podaj własną wartość framerate (np. 42,893)");
                    klatki = double.Parse(Console.ReadLine());
                    break;
            }
            return klatki;
        }

        public static bool flaga = false;
        public static double klatki = 10;
        //Wyciąganie klatek z różnych formatów, przeróbka na czas i pakowanie do struktury.
        static CzasT Czas(string obrobka)
        {
            TimeSpan startZero = new TimeSpan(0, 0, 0);
            CzasT c = new CzasT(startZero, startZero);

            if (RozpoznajFormat(obrobka) == 0) // Format decymalny - frmerate 10 klatek/s
            {
                // Wyciąganie klatek z linii tekstu.
                string[] slowa = obrobka.Split('[', ']');

                double start = double.Parse(slowa[1]);
                double stop = 1;

                //ZABEZPIECZENIE do braku klatki końcowej
                if (slowa[3] == null)
                    slowa[3] = slowa[1] + 90;
                else
                    stop = double.Parse(slowa[3]);

                // Stworzenie struktury zawierającej czas początku i czas końca
                c = new CzasT(ZnacznikCzasu(start, klatki), ZnacznikCzasu(stop, klatki));
            }

            if (RozpoznajFormat(obrobka) == 1)
            {
                // Nowa ilość klatek dla tego formatu.
                if (!flaga)
                {
                    klatki = Framerate();
                    flaga = true;
                }

                // Wyciąganie klatek z linii tekstu.
                string[] slowa = obrobka.Split('{', '}');

                double start = double.Parse(slowa[1]);
                double stop = 1;

                //ZABEZPIECZENIE do braku klatki końcowej
                if (slowa[3] == null)
                    slowa[3] = slowa[1] + 90;
                else
                    stop = double.Parse(slowa[3]);

                // Stworzenie struktury zawierającej czas początku i czas końca
                c = new CzasT(ZnacznikCzasu(start, klatki), ZnacznikCzasu(stop, klatki));
            }

            if (RozpoznajFormat(obrobka) == 2)
            {
                string[] podzial;
                podzial = obrobka.Split(':');

                TimeSpan start = new TimeSpan(int.Parse(podzial[0]), int.Parse(podzial[1]), int.Parse(podzial[2]));
                TimeSpan wyswietlenie = new TimeSpan(0, 0, 4); // Czas wyświetlenia napisu ustawiony na 4s
                TimeSpan koniec = start + wyswietlenie;
                c = new CzasT(start, koniec);
            }

            if (RozpoznajFormat(obrobka) == 3)
            {
                Console.WriteLine("BŁĄD: Wygląda na to, że ten format zapisu napisów nie jest obsługiwany.\nZamknij program.");
                Console.ReadKey();
            }

            return c;
        }

        //Wypisywanie nagłówka powitalnego
        static void WypiszNaglowek()
        {
            Console.WriteLine("|| ------------------------------------------------------------------ ||");
            Console.WriteLine("||                   Konwerter napisów do TV v2.4                     ||");
            Console.WriteLine("|| ------------------------------------------------------------------ ||");
            Console.WriteLine();
        }

        // Cały mechanizm łącznie z podmianą znaków
        static void DlaTXT(string[] args)
        {
            string zapisywany = args[0].Remove(args[0].Length - 4);
            // string zapisywany = "D:\\Bartek\\PRACA\\C#\\Podmiana znaku\\Podmiana znaku\\bin\\Debug\\";
            string obrobka;
            int nr_napisu = 0;
            CzasT czasy;
            Console.WriteLine();
            Console.WriteLine("Plik do przerobienia:");
            Console.WriteLine(args[0].ToString() + "\n");


            try
            {
                // Utworzenie readera i writera
                StreamReader str = new StreamReader(args[0], Encoding.Default);
                StreamWriter stwr = new StreamWriter(zapisywany + ".srt");

                // Analiza linia po linii z pliku źródłowego
                while (true)
                {
                    obrobka = str.ReadLine();        // Odczytanie z pliku
                    if (obrobka == null)            // ZABEZPIECZENIE brak kolejnego napisu
                        break;
                    czasy = Czas(obrobka);          // Wyciągnięcie czasów do struktury
                    obrobka = SamTekst(obrobka);    // Podmiana znaków, usuniecie 
                    nr_napisu++;

                    // Zapis do pliku nowej linijki tekstu
                    stwr.WriteLine(nr_napisu);
                    string doWypisania = string.Format("{0}:{1}:{2},{3} --> {4}:{5}:{6},{7}", DodanieZera(czasy.GetT1().Hours), DodanieZera(czasy.GetT1().Minutes), DodanieZera(czasy.GetT1().Seconds), DodanieDrugiegoZera(czasy.GetT1().Milliseconds), DodanieZera(czasy.GetT2().Hours), DodanieZera(czasy.GetT2().Minutes), DodanieZera(czasy.GetT2().Seconds), DodanieDrugiegoZera(czasy.GetT2().Milliseconds));
                    stwr.WriteLine(doWypisania);
                    stwr.WriteLine(obrobka);
                    stwr.WriteLine();

                };

                str.Close();
                stwr.Flush();
                stwr.Close();
                Console.WriteLine("\n-------------------");
                Console.WriteLine("GOTOWE I ZAPISANE!\nNazwa pliku: {0}.srt\n\nKliknij cokolwiek by zakończyć ;)", zapisywany.Substring(zapisywany.LastIndexOf('\\') + 1, zapisywany.Length - zapisywany.LastIndexOf('\\') - 1));
            }

            catch (Exception e)
            {
                Console.WriteLine("ZJEBAŁO SIĘ!! BŁĄD:");
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
        }

        // Tylko podmiana znaków i zaps do nowego pliku
        static void DlaSRT(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine("Plik do przerobienia:");
            Console.WriteLine(args[0].ToString() + "\n");
            Console.WriteLine("DLA FORMATU .SRT TYLKO OPCJA PODMIANY POLSKICH ZNAKÓW.\n");

            
            string sciezkaDostepu = args[0].ToString();
            string sciezkaBackupu = sciezkaDostepu + "-backup";
            System.IO.File.Move(sciezkaDostepu, sciezkaBackupu);
            string calosc = "";

            try
            {
                // Utworzenie readera i writera
                StreamReader str = new StreamReader(sciezkaBackupu, Encoding.Default);
                StreamWriter stwr = new StreamWriter(sciezkaDostepu, false, Encoding.Default);

                calosc = str.ReadToEnd();
                calosc = PodmianaZnakow(calosc);
                stwr.Write(calosc);

                str.Dispose();
                str.Close();
                
                //Zmiana nazwy starego pliku na jakiś backup i wrzucenie nowej zawartości pod starą nazwę
                
                stwr.Flush();
                stwr.Close();
                Console.WriteLine("\n-------------------");
                Console.WriteLine("GOTOWE I ZAPISANE!\nNazwa pliku: {0}\n\nKliknij cokolwiek by zakończyć ;)", sciezkaDostepu.Substring(sciezkaDostepu.LastIndexOf('\\') + 1, sciezkaDostepu.Length - sciezkaDostepu.LastIndexOf('\\') - 1));
            }

            catch (Exception e)
            {
                Console.WriteLine("ZJEBAŁO SIĘ!! BŁĄD:");
                Console.WriteLine(e.ToString());
            }
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            //Encoding enc = Encoding.GetEncoding("Windows-1250"); //dla UTF-8
            //Encoding enc = Encoding.GetEncoding("Windows-1252"); //dla ANSI
            WypiszNaglowek();

            if (args.Length == 0)
            {
                Console.WriteLine("\nBy przerobić plik, po prostu przeciągnij go na ikonkę programu.");
                Console.ReadKey();
            }
            else
            {
                if (args[0].Substring(args[0].Length - 3, 3) == "txt")
                    DlaTXT(args);
                else if (args[0].Substring(args[0].Length - 3, 3) == "srt")
                    DlaSRT(args);
                else
                {
                    Console.WriteLine("Nieobsługiwany format pliku!\nKliknij cokolwiek, by zakończyć.");
                    Console.ReadKey();
                }
            }
        }
    }
}
