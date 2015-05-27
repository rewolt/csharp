using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Needleman_Wunsc_Algorytm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            tbMatch.Text = "2";
            tbMiss.Text = "-1";
            tbGap.Text = "-2";

            lWynik.Text = "----\n\n----";
            label6.Text = "-";
        }


        /**
         * @brief Tworzenie macierzy wyników
         * 
         * Metoda przyjmuje dwa ciągi znaków i na ichpodstawie tworzy macierz oraz wstępnie wypełnia ją zerami.
         */
        private int [,] StworzMacierzWynikow(string referencja, string aline)
        {
            int referencjaLicznik = referencja.Length + 1;
            int alineLicznik = aline.Length + 1;

            int[,] macierzWyników = new int[alineLicznik, referencjaLicznik];

            // Wypełnienie zerami pierwszego wiersza i pierwszej kolumny
            for (int i = 0; i < alineLicznik; i++)
            {
                macierzWyników[i, 0] = 0;
            }

            for (int j = 0; j < referencjaLicznik; j++)
            {
                macierzWyników[0, j] = 0;
            }
            return macierzWyników;
        }
        /**
         * @brief Wypełnianie macierzy punktacją dopasowań
         * 
         * Metoda przyjmuje wcześniej stworzoną tablicę oraz jej dwa stringi oraz zwraca tablicę wypełnioną punktacją dopasowań
         */
        private int[,] WypelnijMacierzWynikow(int[,] macierzWyników, string referencja, string align, int match, int miss, int gap)
        {
            int alignLicznik = align.Length + 1;
            int referencjaLicznik = referencja.Length + 1;

            for (int i = 1; i < alignLicznik; i++)
            {
                for (int j = 1; j < referencjaLicznik; j++)
                {
                    int wynikDiag = 0;
                    if (referencja.Substring(j - 1, 1) == align.Substring(i - 1, 1))
                        wynikDiag = macierzWyników[i - 1, j - 1] + match;
                    else
                        wynikDiag = macierzWyników[i - 1, j - 1] + miss;

                    int wynikLewa = macierzWyników[i, j - 1] + gap;
                    int wynikPrawa = macierzWyników[i - 1, j] + gap;

                    int maksymalnyWynik = Math.Max(Math.Max(wynikDiag, wynikLewa), wynikPrawa);

                    macierzWyników[i, j] = maksymalnyWynik;
                }
            }
            return macierzWyników;
        }

        /**
         * @brief Dopasowywanie wyrazów
         * 
         * Metoda przyjmuje wszystkie zmienne i zwraca 2 wyrazy dopasowane do siebie na podstawie macierzy wyników i punktacji.
         */
        private string[] DopasujWyrazy(int[,] macierzWyników, string referencja, string align, int match, int miss, int gap)
        {
            char[] alignZnaki = align.ToCharArray();
            char[] referencjaZnaki = referencja.ToCharArray();

            string dopasowanieA = string.Empty;
            string dopasowanieB = string.Empty;
            int i = align.Length;
            int j = referencja.Length;
            while (i > 0 && j > 0)
            {
                int wynikDiag = 0;
                if (alignZnaki[i - 1] == referencjaZnaki[j - 1])
                    wynikDiag = match;
                else
                    wynikDiag = miss;

                if (i > 0 && j > 0 && macierzWyników[i, j] == macierzWyników[i - 1, j - 1] + wynikDiag)
                {
                    dopasowanieA = referencjaZnaki[j - 1] + dopasowanieA;
                    dopasowanieB = alignZnaki[i - 1] + dopasowanieB;
                    i = i - 1;
                    j = j - 1;
                }
                else if (j > 0 && macierzWyników[i, j] == macierzWyników[i, j - 1] +gap)
                {
                    dopasowanieA = referencjaZnaki[j - 1] + dopasowanieA;
                    dopasowanieB = "-" + dopasowanieB;
                    j = j - 1;
                }
                else if (i > 0 && macierzWyników[i, j] == macierzWyników[i - 1, j] +gap)
                {
                    dopasowanieA = "-" + dopasowanieA;
                    dopasowanieB = alignZnaki[i - 1] + dopasowanieB;
                    i = i - 1;
                }
            } 
            string [] dopasowania = {dopasowanieA, dopasowanieB};
            return dopasowania;
        }

        /**
         * @brief Dobranie znaczników do wyniku porównania
         * 
         * Metoda wypisuje dodatkowe znaczniki oznaczające kolejno:
         * ' ' (pusty znak) - gap - brak dopasowania
         *  : - miss - niepoprawne dopasowanie
         *  | - match - poprawne dopasowanie
         */
        private string DobierzZnaczniki(string agl1, string agl2)
        {
            string znaczniki = "";
            for (int i = 0; i < agl1.Length; i++ )
            {
                if (agl1[i] == agl2[i])
                    znaczniki += "|";
                else if ((agl1[i].Equals('-') && !agl2[i].Equals('-')) || (!agl1[i].Equals('-') && agl2[i].Equals('-')))
                    znaczniki += " ";
                else
                    znaczniki += ":";
                    
            }
            return znaczniki;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Sprawdzenie, czy wszystkie pola są wypełnione
            if (tb1.Text == "" || tb2.Text == "" || tbGap.Text=="" || tbMatch.Text=="" || tbMiss.Text=="")
            {
                MessageBox.Show("Przynajmniej jedno z pól tekstowych jest puste. Wpisz poprawne ciągi znaków i punktację.", "BŁĄD!" , MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Podmiana na wielkie litery
            string text1 = tb1.Text;
            string text2 = tb2.Text;
            text1 = text1.ToUpper();
            text2 = text2.ToUpper();
            tb1.Text = text1;
            tb2.Text = text2;

            // Stworzenie punktacji
            int match = 0;
            int miss = 0;
            int gap = 0;

            // Sprawdzenie, czy wartości punktacji są poprawne
            try
            {
                match = int.Parse(tbMatch.Text);
                miss = int.Parse(tbMiss.Text);
                gap = int.Parse(tbGap.Text);
            }
            catch
            {
                MessageBox.Show("Niepoprawne wartości w punktacji. Dozwolone tylko liczby całkowite.", "BŁĄD!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            
            // Wykorzystanie kolejnych kroków do obliczeń
            var macierzWynikow = StworzMacierzWynikow(text1, text2);
            var wypelnionaMacierzWynikow = WypelnijMacierzWynikow(macierzWynikow, text1, text2, match, miss,gap);
            var dopasowaneWyrazy = DopasujWyrazy(wypelnionaMacierzWynikow, text1, text2, match, miss, gap);
            var znaczniki = DobierzZnaczniki(dopasowaneWyrazy[0], dopasowaneWyrazy[1]);

            // Prezentacja wyniku
            lWynik.Text = dopasowaneWyrazy[0] + "\n" + znaczniki + "\n" + dopasowaneWyrazy[1];
            label6.Text = wypelnionaMacierzWynikow[wypelnionaMacierzWynikow.GetLength(0)-1, wypelnionaMacierzWynikow.GetLength(1)-1].ToString();
            Clipboard.SetText(lWynik.Text);
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
