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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bt_konwertuj_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("chuj");
        }

        private void bt_wczytaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Plik .tex|*.tex";
            openFile.Title = "Wybierz plik do odczytu...";
            openFile.ShowDialog();
            odczytLokalizacja = openFile.FileName;
            tb_wczytaj.Text = odczytLokalizacja;
            updateLokalizacji();
        }

        private void bt_zapisz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Plik .tex|*.tex";
            saveFile.Title = "Zapisz plik jako...";
            saveFile.ShowDialog();
            zapisLokalizacja = saveFile.FileName;
            tb_zapisz.Text = zapisLokalizacja;
        }

        private void bt_zapisz_backup_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Plik .tex|*.tex";
            saveFile.Title = "Zapisz plik jako...";
            saveFile.ShowDialog();
            zapisBackupLokalizacja = saveFile.FileName;
            tb_zapisz_backup.Text = zapisLokalizacja;
        }

        private void updateLokalizacji()
        {
            zapisLokalizacja = odczytLokalizacja;
            zapisBackupLokalizacja = zapisLokalizacja;
            zapisBackupLokalizacja = zapisBackupLokalizacja.Insert(zapisBackupLokalizacja.LastIndexOf('.'), "_BACKUP");
            tb_zapisz.Text = zapisLokalizacja;
            tb_zapisz_backup.Text = zapisBackupLokalizacja;
        }
    }
}
