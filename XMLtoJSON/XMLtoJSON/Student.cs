using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoJSON
{
    class Student
    {

        List<Przedmiot> przedmioty = new List<Przedmiot>();

        public int Id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public List<Przedmiot> Przedmioty
        {
            get { return przedmioty; }
            set { przedmioty = value; }
        }

        public double SredniaWszystkichOcen()
        {
            int ilosc = 0;
            double suma = 0;
            przedmioty.ForEach(przedmiot => przedmiot.Oceny.ForEach(ocena => { suma += ocena.Wartosc; ilosc++; }));
            double srednia = suma / (double)ilosc;
            return srednia;
        }
    }
}
