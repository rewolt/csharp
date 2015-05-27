using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using itek = itk.simple;

namespace SimpleITK_example1
{
    public partial class Form1 : Form
    {
        string lokalizacja = "";

        public Form1()
        {
            InitializeComponent();
           
        }

        private void open_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            //openDialog.DefaultExt = "dcm";
            openDialog.Filter = "Pliki obrazów (*.dcm)|*.dcm";
            openDialog.ShowDialog();
            lokalizacja = openDialog.FileName;
            lLokalizacja.Text = lokalizacja;
        }

        private void bAkcja_Click(object sender, EventArgs e)
        {
            if (lokalizacja == "")
                return;
            try
            {
                itek.ImageFileReader rdr = new itek.ImageFileReader();
                rdr.SetFileName(lokalizacja);
                itek.Image obraz = rdr.Execute();
                obraz.Dispose();
                
            }
            catch (System.Exception ex) { MessageBox.Show(ex.ToString()); }
            
        }
    }
}
