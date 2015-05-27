using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Newtonsoft.Json;


namespace XMLtoJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            XmlDocument dane = new XmlDocument();
            dane.Load(new StreamReader("oceny.xml"));

            List<Student> studenci = ParsujXML(dane);



            string conv = JsonConvert.SerializeObject(studenci);
            Console.Write(conv);

            Console.WriteLine("...sukces...");
            Console.ReadKey();


        }

        public static List<Student> ParsujXML(XmlDocument dokument)
        {
            List<Student> studenci = new List<Student>();

            foreach (XmlNode nodePrzedmiot in dokument.DocumentElement)
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
