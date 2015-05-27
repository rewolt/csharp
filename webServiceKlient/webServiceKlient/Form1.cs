using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace webServiceKlient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //serwisy.HelloWorldRequest serwis = new serwisy.HelloWorldRequest();
           // richTextBox1.Text = serwis.ToString();

            serwisy.ModifySoapClient soap = new serwisy.ModifySoapClient();
            richTextBox1.Text = soap.HelloWorld();

            richTextBox1.Text = richTextBox1.Text + soap.Dodawanie(5, 7).ToString();
            richTextBox1.Text = richTextBox1.Text + soap.Modyfikacja("siema");
            //serwisy.ModyfikacjaResponseBody serw1 = new serwisy.ModyfikacjaResponseBody();
            //richTextBox1.Text = serw1.ModyfikacjaResult;



        }
    }
}
