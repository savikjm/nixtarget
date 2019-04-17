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
    public partial class OldValues : Form
    {
        public Dictionary<string, Dictionary<string, string>> listOldValues; 
        public OldValues()
        {
            InitializeComponent();
        }

        public void PopulateList()
        {
            checkedListBox1.Items.Clear();
            foreach (var item in listOldValues)
            {
                checkedListBox1.Items.Add(item.Key);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void update_btn_Click(object sender, EventArgs e)
        {
            var tmpList = checkedListBox1.CheckedItems;
            foreach (var item in tmpList)
            {
                var res = DALHelper.UpdateCompanyReport(listOldValues[item.ToString()]);
                if (res)
                {
                    listOldValues.Remove(item.ToString());
                }
            }
            PopulateList();
        }
    }
}
