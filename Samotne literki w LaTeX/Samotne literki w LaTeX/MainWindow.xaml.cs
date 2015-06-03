using Microsoft.Win32;
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

namespace Samotne_literki_w_LaTeX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string odczytLokalizacja = "";
        string zapisLokalizacja = "";
        string zapisBackupLokalizacja = "";
        string[] args;

        public MainWindow(string [] args)
        {
            this.args = args;
            InitializeComponent();
            if (args.Length != 0)
            {
                odczytLokalizacja = args[0];
                updateLokalizacji();
                infoText.Content = "Zmień domyślne nazwy lub konwertuj od razu";
            }
        }

        private void bt_konwertuj_Click(object sender, RoutedEventArgs e)
        {
            if (odczytLokalizacja.Equals(""))
                return;
            string tekst = Pliki.Odczytaj(odczytLokalizacja);
            Pliki.Zapisz(tekst, zapisBackupLokalizacja);
            Konwerter konw = new Konwerter(tekst);
            Pliki.Zapisz(konw.Konwertuj(), zapisLokalizacja);
            infoText.FontSize = 16;
            infoText.Content = "POMYSLNIE PRZEKONWERTOWANO I ZAPISANO :)";
        }

        private void bt_wczytaj_Click(object sender, RoutedEventArgs e)
        {
            odczytLokalizacja = Pliki.DialogOtworzPlik();
            tb_wczytaj.Text = odczytLokalizacja;
            updateLokalizacji();
        }

        private void bt_zapisz_Click(object sender, RoutedEventArgs e)
        {

            zapisLokalizacja = Pliki.DialogZapiszPlik();
            tb_zapisz.Text = zapisLokalizacja;
        }

        private void bt_zapisz_backup_Click(object sender, RoutedEventArgs e)
        {
            zapisBackupLokalizacja = Pliki.DialogZapiszPlik();
            tb_zapisz_backup.Text = zapisLokalizacja;
        }

        private void updateLokalizacji()
        {
            zapisLokalizacja = odczytLokalizacja;
            zapisBackupLokalizacja = zapisLokalizacja;
            zapisBackupLokalizacja = zapisBackupLokalizacja.Insert(zapisBackupLokalizacja.LastIndexOf('.'), String.Format("(BACKUP[{0:00}.{1:00}.{2}][{3}.{4}.{5}])", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
            tb_wczytaj.Text = odczytLokalizacja;
            tb_zapisz.Text = zapisLokalizacja;
            tb_zapisz_backup.Text = zapisBackupLokalizacja;
        }
    }
}