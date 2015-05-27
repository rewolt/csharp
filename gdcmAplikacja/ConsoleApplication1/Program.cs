using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // typ wyszukiwania (rozpoczynamy od pacjenta)
            gdcm.ERootType typ = gdcm.ERootType.ePatientRootType;

            // do jakiego poziomu wyszukujemy 
            gdcm.EQueryLevel poziom = gdcm.EQueryLevel.ePatient; // zobacz inne 

            // klucze (filtrowanie lub określenie, które dane są potrzebne)
            gdcm.KeyValuePairArrayType klucze = new gdcm.KeyValuePairArrayType();
            gdcm.KeyValuePairType klucz1 = new gdcm.KeyValuePairType(new gdcm.Tag(0x0010, 0x0020), "2593/05/M"); // NIE WOLNO TU STOSOWAC *; tutaj PatientID="01"
            klucze.Add(klucz1);
            
            // skonstruuj zapytanie
            gdcm.BaseRootQuery zapytanie = gdcm.CompositeNetworkFunctions.ConstructQuery(typ, poziom, klucze, true);

            // sprawdź, czy zapytanie spełnia kryteria
            if (!zapytanie.ValidateQuery())
            {
                Console.WriteLine("Błędne zapytanie");
                return;
            }

            // przygotuj katalog na wyniki
            String odebrane = System.IO.Path.Combine(".", "odebrane");

            if (!System.IO.Directory.Exists(odebrane))
                System.IO.Directory.CreateDirectory(odebrane);

            String dane = System.IO.Path.Combine(odebrane, System.IO.Path.GetRandomFileName());
            System.IO.Directory.CreateDirectory(dane);
            

            // wykonaj zapytanie - pobierz do katalogu _dane_
            bool stan = gdcm.CompositeNetworkFunctions.CMove("127.0.0.1", 10100, zapytanie, 10104, "KLIENTL", "ARCHIWUM", dane);
            
            // sprawdź stan
            if (!stan)
            {
                Console.WriteLine("Nie działa");
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
                    Console.WriteLine("opuszczam plik {0}", plik);
                    continue;
                }

                // przekonwertuj na "znany format"
                gdcm.Bitmap bmjpeg2000 = pxmap2jpeg2000(reader.GetPixmap());
                // przekonwertuj na .NET bitmapy
                Bitmap [] X = gdcmBitmap2Bitmap(bmjpeg2000);
                // zapisz
                for (int i = 0; i < X.Length; i++)
                {
                    String name = String.Format("{0}_warstwa{1}.jpg", plik, i);
                    X[i].Save(name);
                }

            }
        }

        // Konwertuj Bitmapę GDCM na Bitmapę .NET, przy czym:
        // 1. załóż kodowanie LE, monochromatyczne
        // 2. każdą z bitmap przeskaluj do 0-255 korzystając z wartości maksymalnej
        public static Bitmap [] gdcmBitmap2Bitmap(gdcm.Bitmap bmjpeg2000)
        {
                // przekonwertuj teraz na bitmapę C#
                uint cols = bmjpeg2000.GetDimension(0);
                uint rows = bmjpeg2000.GetDimension(1);
                uint layers = bmjpeg2000.GetDimension(2);

                // wartość zwracana - tyle obrazków, ile warstw
                Bitmap [] ret = new Bitmap[layers];


                // bufor
                byte [] bufor = new byte[bmjpeg2000.GetBufferLength()];
                if (!bmjpeg2000.GetBuffer(bufor))
                    throw new Exception("błąd pobrania bufora");

                // w strumieniu na każdy piksel 2 bajty; tutaj LittleEndian (mnie znaczący bajt wcześniej)
                for (uint l = 0; l < layers; l++)
                {
                    Bitmap X = new Bitmap((int)cols, (int)rows);
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
                            X.SetPixel(c, r, Color.FromArgb(f, f, f));
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

    }
}

