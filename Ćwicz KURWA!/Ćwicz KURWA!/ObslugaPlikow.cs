using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Ćwicz_KURWA_
{
    class ObslugaPlikow
    {
        public static void StworzJesliBrak(string plik){
            if (!File.Exists(plik))
                File.Create(plik);
        }

        public static Cwiczenie[] ZwrocCwiczenia(string plik)
        {
            // Deklarowanie zmiennych
            StreamReader sr = new StreamReader(plik);
            List<string> linie = new List<string>();
            Cwiczenie[] listaCwiczen;
            string linia;

            // Odczyt ćwiczeń z pliku ćwiczenia.txt
            while ((linia = sr.ReadLine()) != null)
            {
                if ((!linia.StartsWith("#")) && (linia.Length != 0))
                    linie.Add(linia);
            }

            // Tworzenie obiektów ćwiczeń do tablicy
            int id = 0;
            listaCwiczen = new Cwiczenie[linie.Count];
            foreach (string l in linie)
            {
                string[] czesci = l.Split(' ');
                Cwiczenie cw = new Cwiczenie(id, czesci[0], czesci[1]);
                listaCwiczen[id] = cw;
                id++;
            }

            return listaCwiczen;
        }
    }
}
