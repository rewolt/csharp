using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ćwicz_KURWA_
{
    class Cwiczenie
    {
        int id;
        string nazwa;
        List<TimeSpan> listaCzasów;

        public Cwiczenie() {
            this.listaCzasów = new List<TimeSpan>();
            this.nazwa = "";
        }
        public Cwiczenie(int id, string nazwa, string czasowki)
        {
            this.id = id;
            this.listaCzasów = new List<TimeSpan>();
            this.nazwa = nazwa;
            string []podzieloneCzasowki = czasowki.Split('-');
            foreach (string sekundy in podzieloneCzasowki)
            {
                TimeSpan czas = new TimeSpan(0, 0, int.Parse(sekundy));
                listaCzasów.Add(czas);
            }
            
        }
        public void DodajCzas (string czas)
        {
            TimeSpan time = new TimeSpan(0, 0, int.Parse(czas));
            listaCzasów.Add(time);
        }

        // Zwraca opis: id, nazwa, całkowity czas, poszczególne czasy
        public string[] ZwrocOpis()
        {
            string[] opis = new string[4];
            opis[0] = id.ToString();
            opis[1] = nazwa;

            TimeSpan sumaCzasow = new TimeSpan();
            foreach (TimeSpan czas in listaCzasów)
            {
                opis[3] = czas.TotalSeconds.ToString() + "-";
                sumaCzasow.Add(czas);
            }
            opis[2] = sumaCzasow.Hours.ToString() + ":" + sumaCzasow.Minutes.ToString() + ":" + sumaCzasow.Seconds.ToString();

            return opis;

        }
        public string Nazwa { get { return nazwa; } set { nazwa = value; } }
        public List<TimeSpan> ListaCzasów { get { return listaCzasów; } set { listaCzasów = value; } }
    }
}
