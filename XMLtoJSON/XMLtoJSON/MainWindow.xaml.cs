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
using Microsoft.Win32;
using System.Xml;
using Newtonsoft.Json;

namespace XMLtoJSON
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string odczytLokalizacja = "";
        private string zapisLokalizacja = "";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void bLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Plik oceny.xml|oceny.xml";
            openFile.Title = "Wybierz plik do odczytu...";
            openFile.ShowDialog();
            odczytLokalizacja = openFile.FileName;
            tbLoad.Text = odczytLokalizacja;
        }

        private void bSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Plik .json|*.json";
            saveFile.Title = "Zapisz plik jako...";
            saveFile.ShowDialog();
            zapisLokalizacja = saveFile.FileName;
            tbSave.Text = zapisLokalizacja;

        }

        private void bAuthor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bartłomiej Tomczak\nbartlomiej.tomczak@hotmail.com\n\nAplikacja stworzona na potrzeby procesu rekrutacyjnego", "O autorze...");
        }

        private void bConvert_Click(object sender, RoutedEventArgs e)
        {
            if (zapisLokalizacja == ""){
                MessageBox.Show("Podaj lokalizację pliku do odczytania.", "Nie podano pliku", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else if (odczytLokalizacja == "")
            {
                MessageBox.Show("Podaj lokalizację pliku do zapisania.", "Nie podano pliku", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            XmlDocument data = new XmlDocument();
            data.Load(new StreamReader(odczytLokalizacja));

            List<Student> studenci = ParsujXML(data);
            List<StatystykaStudentow> statystykaStudentow = StworzStatystykiStudentow(studenci);
            List<StatystykaPrzedmiotow> statystykaPrzedmiotow = StworzStatystykiPrzedmiotow(studenci);

            string statystykaStudentowTekst = JsonConvert.SerializeObject(statystykaStudentow, Newtonsoft.Json.Formatting.Indented);
            string statystykaPrzedmiotowTekst = JsonConvert.SerializeObject(statystykaPrzedmiotow, Newtonsoft.Json.Formatting.Indented);
            string conv = FormatowanieNapisow(statystykaStudentowTekst, statystykaPrzedmiotowTekst);

            StreamWriter sr = new StreamWriter(zapisLokalizacja);
            sr.Write(conv);
            sr.Flush();
            sr.Close();
            
            MessageBox.Show("Poprawnie skonwertowano i zapisano plik", "Operacja zakończona sukcesem", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        private static string FormatowanieNapisow(string statystykaStudentowTekst, string statystykaPrzedmiotowTekst)
        {
            string studenciNaglowek = "{\n\"studenci\": ";
            string przedmiotyNaglowek = "\"przedmioty\": ";

            string calyTekst = studenciNaglowek + statystykaStudentowTekst + ",\n" + przedmiotyNaglowek + statystykaPrzedmiotowTekst + "}";
            return calyTekst;

        }


        private static List<StatystykaPrzedmiotow> StworzStatystykiPrzedmiotow(List<Student> studenci)
        {
            List<StatystykaPrzedmiotow> statystykaPrzedmiotow = new List<StatystykaPrzedmiotow>();
            List<Przedmiot> listaPrzedmiotow = new List<Przedmiot>();
            studenci.ForEach(student => listaPrzedmiotow.AddRange(student.Przedmioty));

            if (listaPrzedmiotow.Count == 0)
                return statystykaPrzedmiotow;

            while (listaPrzedmiotow.Count != 0)
            {
                List<Przedmiot> listaPowtorzonegoPrzedmiotu = new List<Przedmiot>();

                // Przepisz pierwszy element do nowej listy i usuń go z poprzedniej
                listaPowtorzonegoPrzedmiotu.Add(listaPrzedmiotow[0]);
                listaPrzedmiotow.RemoveAt(0);

                // Znajdź resztę elementów o tej samej nazwie i przepisz je oraz usuń z poprzedniej listy
                listaPowtorzonegoPrzedmiotu.AddRange(listaPrzedmiotow.FindAll(prz => prz.Nazwa == listaPowtorzonegoPrzedmiotu[0].Nazwa));
                listaPrzedmiotow.RemoveAll(prz => prz.Nazwa == listaPowtorzonegoPrzedmiotu[0].Nazwa);

                // Wypełnij obiekt statystyki i wrzuć do listy
                StatystykaPrzedmiotow statystykaPrzedmiotu = new StatystykaPrzedmiotow();
                statystykaPrzedmiotu.Przedmiot = listaPowtorzonegoPrzedmiotu[0].Nazwa;
                statystykaPrzedmiotu.LiczbaStudentow = listaPowtorzonegoPrzedmiotu.Count;
                listaPowtorzonegoPrzedmiotu.ForEach(przedmiot =>
                {
                    statystykaPrzedmiotu.LiczbaOcenStudentow += przedmiot.LiczbaOcen();
                    statystykaPrzedmiotu.SredniaOcenStudentow += przedmiot.SredniaOcen();
                });
                statystykaPrzedmiotu.SredniaOcenStudentow = statystykaPrzedmiotu.SredniaOcenStudentow / listaPowtorzonegoPrzedmiotu.Count;

                statystykaPrzedmiotow.Add(statystykaPrzedmiotu);
            }


            return statystykaPrzedmiotow;
        }

        private static List<StatystykaStudentow> StworzStatystykiStudentow(List<Student> studenci)
        {
            List<StatystykaStudentow> listaStatystyk = new List<StatystykaStudentow>();

            studenci.ForEach(student =>
            {
                StatystykaStudentow statystykaStudenta = new StatystykaStudentow();
                statystykaStudenta.Imie = student.Imie;
                statystykaStudenta.Nazwisko = student.Nazwisko;
                statystykaStudenta.Srednia_wszystkich_ocen = student.SredniaWszystkichOcen();

                student.Przedmioty.ForEach(przedmiot =>
                {
                    StatystykaOceny statystykaOceny = new StatystykaOceny();
                    statystykaOceny.Przedmiot = przedmiot.Nazwa;
                    statystykaOceny.LiczbaOcen = przedmiot.LiczbaOcen();
                    statystykaOceny.Srednia = przedmiot.SredniaOcen();
                    statystykaStudenta.Oceny.Add(statystykaOceny);
                });

                listaStatystyk.Add(statystykaStudenta);
            });

            return listaStatystyk;
        }

        private static List<Student> ParsujXML(XmlDocument xmlDocument)
        {
            List<Student> studenci = new List<Student>();

            foreach (XmlNode nodePrzedmiot in xmlDocument.DocumentElement)
            {

                foreach (XmlNode nodeStudent in nodePrzedmiot.ChildNodes)
                {
                    // Zwróć studenta, by wpisać mu przedmiot, inaczej stwórz nowego.
                    Student student;
                    if (!studenci.Exists(st => st.Id == int.Parse(nodeStudent.Attributes["id"].InnerText)))
                    {
                        student = new Student();
                        student.Id = int.Parse(nodeStudent.Attributes["id"].InnerText);
                        student.Imie = nodeStudent.Attributes["imie"].InnerText;
                        student.Nazwisko = nodeStudent.Attributes["nazwisko"].InnerText;
                    }
                    else
                    {
                        student = studenci.Find(st => st.Id == int.Parse(nodeStudent.Attributes["id"].InnerText));
                        studenci.Remove(student);
                    }


                    Przedmiot przedmiot = new Przedmiot();
                    przedmiot.Nazwa = nodePrzedmiot.Attributes["nazwa"].InnerText;

                    List<Ocena> oceny = new List<Ocena>();
                    foreach (XmlNode nodeOcena in nodeStudent.ChildNodes)
                    {
                        Ocena ocena = new Ocena();
                        ocena.Typ = nodeOcena.Attributes["typ"].InnerText;
                        string[] data = nodeOcena.Attributes["data"].InnerText.Split('-');
                        ocena.Data = new DateTime(int.Parse(data[0]), int.Parse(data[1]), int.Parse(data[2]));
                        ocena.Wartosc = double.Parse(nodeOcena.InnerText.Trim().Replace('.', ','));
                        oceny.Add(ocena);
                        przedmiot.Oceny = oceny;
                    }

                    student.Przedmioty.Add(przedmiot);
                    studenci.Add(student);


                }

            }

            return studenci;
        }
    }
}
