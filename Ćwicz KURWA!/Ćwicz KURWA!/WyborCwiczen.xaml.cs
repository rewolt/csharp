using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace Ćwicz_KURWA_
{
    /// <summary>
    /// Interaction logic for WyborCwiczen.xaml
    /// </summary>
    public partial class WyborCwiczen : Window
    {
        public WyborCwiczen()
        {
            InitializeComponent();

            ObslugaPlikow.StworzJesliBrak("ćwiczenia.txt");
            Cwiczenie[] cwiczenia = ObslugaPlikow.ZwrocCwiczenia("ćwiczenia.txt");
            
            // Wpisywanie obietków do lWybieranych
            for (int i=0; i<cwiczenia.Count(); i++)
            {                
                LWybieranych.Items.Add(cwiczenia[i].ZwrocOpis());
            }
        }
    }
}
