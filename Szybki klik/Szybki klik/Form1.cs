using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;

using System.IO;

namespace Szybki_klik
{
    public partial class Form1 : Form
    {
        List<long> lista = new List<long>();

        Stopwatch sw = new Stopwatch();
        TimeSpan ts = new TimeSpan();
        long ms = 0;

        public Form1()
        {
            InitializeComponent();
            richTextBox2.KeyDown += richTextBox2_KeyDown;
            buttonDynks.Click += buttonDynks_Click;
        }

        void buttonDynks_Click(object sender, EventArgs e)
        {
            lista.Clear();
            sw.Stop();
            sw.Reset();
        }

        void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (lista.Count == 0)
            {
                sw.Start();
            }

            if (sw.IsRunning)
            {

                sw.Stop();
                ms = sw.ElapsedMilliseconds;
                lista.Add(ms);
                sw.Reset();
                sw.Start();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string str = "";
            //int j = 0;
            //streamwriter sw = new streamwriter("chuj.csv");

            //sw.writeline(";");
            //string[] tab1 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "w", "y", "z" };
            ////string[] tab2 = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "r", "s", "t", "u", "w", "y", "z" };

            //for (int i = 0; i < tab1.length; i++)
            //{
            //    j = i;
            //    for (; j < tab1.length; j++)
            //    {
            //        if (i != j)
            //        {
            //            // str += string.format("{0}:{1}\n", tab1[i], tab1[j]);
            //            sw.writeline("{0}:{1};", tab1[i], tab1[j]);
            //        }

            //    }

            //}


            ////messagebox.show(str);
            //sw.close();

            string str = "";

            foreach (var item in lista)
            {
                str += item + "\n";
            }

            MessageBox.Show(str);
        }
    }
}
