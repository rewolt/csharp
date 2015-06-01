using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoJSON
{
    class Przedmiot
    {
        public string Nazwa { get; set; }
        public List<Ocena> Oceny { get; set; }

        public int LiczbaOcen()
        {
            return Oceny.Count;
        }

        public double SredniaOcen()
        {
            double sredniaOcen = 0;
            double sumaOcen = 0;
            if (Oceny.Count != 0)
            {
                Oceny.ForEach(ocena => { sumaOcen += ocena.Wartosc; });
                sredniaOcen = sumaOcen / Oceny.Count;
            }
            return sredniaOcen;
        }
    }
}
