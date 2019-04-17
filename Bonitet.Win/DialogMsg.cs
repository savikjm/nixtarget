using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using Bonitet.DAL;

namespace Bonitet.Win
{
    public partial class DialogMsg : Form
    {
        public DialogMsg()
        {
            InitializeComponent();
        }


        public void ShowReports(string report)
        {
            textBox1.Text = report;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
