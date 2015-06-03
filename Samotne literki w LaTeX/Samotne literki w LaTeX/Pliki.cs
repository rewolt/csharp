using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samotne_literki_w_LaTeX
{
    class Pliki
    {
        public static string DialogOtworzPlik()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Plik .tex|*.tex";
            openFile.Title = "Wybierz plik do odczytu...";
            openFile.ShowDialog();
            return openFile.FileName;
        }

        public static string DialogZapiszPlik()
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Plik .tex|*.tex";
            saveFile.Title = "Zapisz plik jako...";
            saveFile.ShowDialog();
            return saveFile.FileName;
        }

        public static string Odczytaj(string lokalizacja)
        {
            StreamReader sr = new StreamReader(lokalizacja, Encoding.UTF8);
            string tekst = sr.ReadToEnd();
            sr.Close();
            return tekst;
        }

        public static void Zapisz(string tekst, string lokalizacja)
        {
            StreamWriter sw = new StreamWriter(lokalizacja, false, Encoding.UTF8);
            sw.Write(tekst);
            sw.Flush();
            sw.Close();
        }
    }
}
