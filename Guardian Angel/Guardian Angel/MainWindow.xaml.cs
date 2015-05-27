using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Guardian_Angel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string ConnectionString = "SERVER=www.forumib.netck.pl;DATABASE=c7guardinfo;UID=c7guardian;PASSWORD=angelangel9090;SslMode=Required;";

        public MainWindow()
        {
            InitializeComponent();
            String procesy=null;
            Process[] processes = Process.GetProcesses();
            foreach (var proc in processes)
            {
                if (!string.IsNullOrEmpty(proc.MainWindowTitle))
                    procesy = procesy + proc.MainWindowTitle.ToString()+"\n";
                
            }
            //MessageBox.Show(procesy);


            var mcon = new MySqlConnection(ConnectionString);
            var cmd = mcon.CreateCommand();
            mcon.Open();
            mcon.Close();
            cmd.
            MessageBox.Show("koniec");
        }


    }
}
