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

namespace Przeglądarka_DICOM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        string ip;
        ushort port;
        string aet;
        List<string> bitmapy = new List<string>();
        int nr_obrazu = 1;


        public MainWindow()
        {
            InitializeComponent();
            listaObrazow.IsEnabled = false;
            btNastepny.IsEnabled = false;
            btPoprzedni.IsEnabled = false;
        }

        private void Wyjscie_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Odswiez_Click(object sender, RoutedEventArgs e)
        {
            PolaTekstowe.IsEnabled = false;
            ip = tSerwer.Text;
            port = ushort.Parse(tPort.Text);
            aet = tAET.Text;

            // TESTOWANIE POŁĄCZENIA
            bool stan = gdcm.CompositeNetworkFunctions.CEcho(ip, port, aet, "ARCHIWUM");
            if (!stan)
            {
                MessageBox.Show("Nie można połączyć z serwerem PACS!\nSprawdź adres, port i nazwy AET.", "Problem z połączeniem", MessageBoxButton.OK);
                PolaTekstowe.IsEnabled = true;
                return;
            }


            gdcm.ERootType typ = gdcm.ERootType.ePatientRootType;

            // Do jakiego poziomu odbywa się wyszukiwanie...
            gdcm.EQueryLevel poziom = gdcm.EQueryLevel.ePatient;

            // Klucze do określania, które dane sa potrzebne
            gdcm.KeyValuePairArrayType klucze = new gdcm.KeyValuePairArrayType();
            gdcm.KeyValuePairType klucz = new gdcm.KeyValuePairType(new gdcm.Tag(0x0010, 0x0010), "*");
            klucze.Add(klucz);


            // Konstrukcja zapytania w celi sprawdzenia integralności i poprawności
            gdcm.BaseRootQuery zapytanie = gdcm.CompositeNetworkFunctions.ConstructQuery(typ, poziom, klucze);
            if (!zapytanie.ValidateQuery())
            {
                PolaTekstowe.IsEnabled = true;
                MessageBox.Show("Zapytanie niepoprawne");
                return;
            }

            // WYSŁANIE FIND
            gdcm.DataSetArrayType dane = new gdcm.DataSetArrayType();
            stan = gdcm.CompositeNetworkFunctions.CFind(ip, port, zapytanie, dane, aet, "ARCHIWUM");
            List<string> pacjenci_lista = new List<string>();

            foreach (gdcm.DataSet x in dane)
                pacjenci_lista.Add(x.GetDataElement(new gdcm.Tag(0x0010, 0x0010)).GetValue().toString());

            listaPacjentow.ItemsSource = pacjenci_lista;

        }

        private void listaPacjentow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Zabezpieczenie
            if (listaPacjentow.Items.Count == 0 || listaPacjentow.SelectedItem == null)
                return;

            bitmapy.Clear();
            nr_obrazu = 1;

            string wybanyPacjent = listaPacjentow.SelectedItem.ToString();

            // Testowanie połączenia
            bool stan = gdcm.CompositeNetworkFunctions.CEcho(ip, port, aet, "ARCHIWUM");
            if (!stan)
            {
                MessageBox.Show("Nie można połączyć z serwerem PACS!\nSprawdź adres, port i nazwy AET.", "Problem z połączeniem", MessageBoxButton.OK);
                return;
            }
            
            // Tworzenie klucza do zapytania
            gdcm.ERootType typ = gdcm.ERootType.ePatientRootType;
            gdcm.EQueryLevel poziom = gdcm.EQueryLevel.ePatient;
            gdcm.KeyValuePairArrayType klucze = new gdcm.KeyValuePairArrayType();
            klucze.Add(new gdcm.KeyValuePairType(new gdcm.Tag(0x0010, 0x0010), wybanyPacjent));

            gdcm.BaseRootQuery zapytanie = gdcm.CompositeNetworkFunctions.ConstructQuery(typ, poziom, klucze, true);

            // przygotuj katalog na wyniki
            String odebrane = System.IO.Path.Combine(".", "odebrane");

            if (!System.IO.Directory.Exists(odebrane))
                System.IO.Directory.CreateDirectory(odebrane);

            String dane = System.IO.Path.Combine(odebrane, System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(dane);

            //Zapytanie o obrazy
            stan = gdcm.CompositeNetworkFunctions.CMove("127.0.0.1", 10100, zapytanie, 10104, "KLIENTL", "ARCHIWUM", dane);

            // sprawdź stan
            if (!stan)
            {
                MessageBox.Show("Pobieranie obrazów nie powodło się");
                return;
            }
            
            List<string> pliki = new List<string>(System.IO.Directory.EnumerateFiles(dane));
            foreach (String plik in pliki)
            {
                // przeczytaj pixele
                gdcm.PixmapReader reader = new gdcm.PixmapReader();
                reader.SetFileName(plik);
                if (!reader.Read())
                {
                    MessageBox.Show("opuszczam plik {0}", plik);
                    continue;
                }

                // przekonwertuj na "znany format"
                gdcm.Bitmap bmjpeg2000 = pxmap2jpeg2000(reader.GetPixmap());
                // przekonwertuj na .NET bitmapy
                System.Drawing.Bitmap[] X = gdcmBitmap2Bitmap(bmjpeg2000);
                // zapisz
                for (int i = 0; i < X.Length; i++)
                {
                    String name = String.Format("{0}_warstwa{1}.jpg", plik, i);
                    X[i].Save(name);
                }

            }

            bitmapy.AddRange(System.IO.Directory.EnumerateFiles(dane, "*.jpg"));
            List<string> bitmapy_nazwy = new List<string>();
            bitmapy_nazwy.Clear();
            bitmapy.ForEach(delegate(String nazwa)
                 {                     
                     bitmapy_nazwy.Add("Obraz " + nr_obrazu);
                     nr_obrazu++;
                 });
            listaObrazow.ItemsSource = bitmapy_nazwy;

            listaObrazow.IsEnabled = true;
            btNastepny.IsEnabled = true;
            btPoprzedni.IsEnabled = true;

        }

        private void listaObrazow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Zabezpieczenie
            if (listaObrazow.Items.Count == 0 || listaObrazow.SelectedItem == null)
                return;
            zaladujObraz();
        }




        // Konwertuj Bitmapę GDCM na Bitmapę .NET, przy czym:
        // 1. załóż kodowanie LE, monochromatyczne
        // 2. każdą z bitmap przeskaluj do 0-255 korzystając z wartości maksymalnej
        public static System.Drawing.Bitmap[] gdcmBitmap2Bitmap(gdcm.Bitmap bmjpeg2000)
        {
            // przekonwertuj teraz na bitmapę C#
            uint cols = bmjpeg2000.GetDimension(0);
            uint rows = bmjpeg2000.GetDimension(1);

            uint layers = bmjpeg2000.GetDimension(2);

            // wartość zwracana - tyle obrazków, ile warstw
            System.Drawing.Bitmap[] ret = new System.Drawing.Bitmap[layers];


            // bufor
            byte[] bufor = new byte[bmjpeg2000.GetBufferLength()];
            if (!bmjpeg2000.GetBuffer(bufor))
                throw new Exception("błąd pobrania bufora");

            // w strumieniu na każdy piksel 2 bajty; tutaj LittleEndian (mnie znaczący bajt wcześniej)
            for (uint l = 0; l < layers; l++)
            {
                System.Drawing.Bitmap X = new System.Drawing.Bitmap((int)cols, (int)rows);
                double[,] Y = new double[cols, rows];
                double m = 0;

                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        // współrzędne w strumieniu
                        int j = ((int)(l * rows * cols) + (int)(r * cols) + (int)c) * 2;
                        Y[r, c] = (double)bufor[j + 1] * 256 + (double)bufor[j];
                        // przeskalujemy potem do wartości max.
                        if (Y[r, c] > m)
                            m = Y[r, c];
                    }

                // wolniejsza metoda tworzenia bitmapy
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                        int f = (int)(255 * (Y[r, c] / m));
                        X.SetPixel(c, r, System.Drawing.Color.FromArgb(f, f, f));
                    }
                // kolejna bitmapa
                ret[l] = X;
            }
            return ret;
        }


        // przekonwertuj do formatu bezstratnego JPEG2000
        // bezpośrednio z http://gdcm.sourceforge.net/html/StandardizeFiles_8cs-example.html
        public static gdcm.Bitmap pxmap2jpeg2000(gdcm.Pixmap px)
        {
            gdcm.ImageChangeTransferSyntax change = new gdcm.ImageChangeTransferSyntax();
            change.SetForce(false);
            change.SetCompressIconImage(false);
            change.SetTransferSyntax(new gdcm.TransferSyntax(gdcm.TransferSyntax.TSType.JPEG2000Lossless));

            change.SetInput(px);
            if (!change.Change())
                throw new Exception("Nie przekonwertowano typu bitmapy na jpeg2000");

            return change.GetOutput();

        }

        private void btPoprzedni_Click(object sender, RoutedEventArgs e)
        {
            if (listaObrazow.SelectedIndex == 0)
                return;
            listaObrazow.SelectedIndex = listaObrazow.SelectedIndex - 1;

            zaladujObraz();
        }

        private void btNastepny_Click(object sender, RoutedEventArgs e)
        {
            if (listaObrazow.SelectedIndex == listaObrazow.Items.Count - 1)
                return;
            listaObrazow.SelectedIndex = listaObrazow.SelectedIndex + 1;

            zaladujObraz();

        }

        private void zaladujObraz()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(bitmapy.ElementAt(listaObrazow.SelectedIndex), UriKind.RelativeOrAbsolute);
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.EndInit();
            Wyswietlacz.Source = image;
        }
    }
}
