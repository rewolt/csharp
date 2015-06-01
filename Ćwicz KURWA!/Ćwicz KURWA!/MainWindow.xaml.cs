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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace Ćwicz_KURWA_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ObslugaPlikow.StworzJesliBrak("save.ini");

            lista.Focus();
            
            lista.Items.Add("Siema");
            lista.SelectedIndex = 0;
        }

        private void Pomoc_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Kliknij na stoper, by rozpocząć ćwiczenia. Kliknij ponownie, by spauzować.\nPrzycisk \"Reset\" resetuje program ćwiczeń.\nW celu edycji listy ćwiczeń, wybierz przycisk edycji.", "Pomoc");
        }

        private void Edycja_Click(object sender, RoutedEventArgs e)
        {
            WyborCwiczen wc = new WyborCwiczen();
            wc.Show();
            
        }
    }
}
