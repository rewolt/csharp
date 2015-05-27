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
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace TCP_server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public delegate void Delegata(string Tekst);

    class Server
    {
        public event Delegata zdarzenie;

        MainWindow mainWindow = null;

        private TcpListener tcpListener;
        private Thread listenThread;
        public string Tekst {get; set;}

        private void ListenForClients()
        {
            this.tcpListener.Start();

            while(true)
            {
                // blokowanie, póki klient nie połączy się z serwerem
                TcpClient client = this.tcpListener.AcceptTcpClient();

                //stworzenie wątku zajmującego się komunikacją z klientem
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(client);
            }
        }

        public Server()
        {
            this.tcpListener = new TcpListener(IPAddress.Any, 2324);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            this.listenThread.Start();
        }

        public Server(object Kontrolka)
            : this()
        {
            this.mainWindow = (MainWindow)Kontrolka;
        }


        private void HandleClientComm(object client)
        {
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
                //string tekst = encoder.GetString(message, 0, bytesRead);
                System.Diagnostics.Debug.WriteLine("Klient gada: "+ Tekst);
                
                if(zdarzenie != null)
                {
                    zdarzenie(Tekst);
                }


                byte[] buffer = encoder.GetBytes("SERWER ODPOWIADA: "+ Tekst);
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();
            }

            tcpClient.Close();
        }
    }


    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Server s = new Server(this);

            s.zdarzenie += s_zdarzenie;
            siema.Text=s.Tekst+"\n";


        }

        void s_zdarzenie(string Tekst)
        {
            siema.Text = Tekst;
        }

        private void siema_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
