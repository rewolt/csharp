using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samotne_literki_w_LaTeX
{
    class Konwerter
    {
        string tekst;
        public Konwerter(string tekst)
        {
            this.tekst = tekst;
        }

        public string Konwertuj()
        {
            string skonwertowany="";
            string[] tab = tekst.Split(' ');
            foreach (string slowo in tab)
            {
                if (slowo.Length != 1)
                    skonwertowany += slowo + " ";
                else
                    skonwertowany += slowo + "~";
            }

            return skonwertowany;
        }
    }
}
