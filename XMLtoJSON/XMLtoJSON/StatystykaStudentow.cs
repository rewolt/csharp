using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLtoJSON
{
    class StatystykaStudentow
    {
        List<StatystykaOceny> stat = new List<StatystykaOceny>();
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public double Srednia_wszystkich_ocen { get; set; }
        public List<StatystykaOceny> Oceny
        {
            get { return stat; }
            set { stat = value; }
        }

    }
}
