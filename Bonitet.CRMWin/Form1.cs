using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bonitet.CRMWin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            resultBox.ScrollBars = ScrollBars.Vertical;
            resultBox.WordWrap = true;
        }

        public static void GetCRMKompletenIzvestaj()
        {
            //4328337 - jaca 
            /*
             5171105 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5224195 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5237955 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5491495 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5498465 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5890705 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5953545 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5493005 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5507855 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             5501288 - Субјектот нема доставено годишна сметка или известување дека нема деловна активност во 2014 година.
             */
            var EMBS = "6311792";
            var res = Bonitet.CRM.CRM_ServiceHelper.GetCRM_Account(EMBS, 2014);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var EMBS = embsText.Text.Trim();
            var res = Bonitet.CRM.CRM_ServiceHelper.GetCRM_Account(EMBS, 2014);

            resultBox.Text = res; 
        }

        private void embsText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var EMBS = embsText.Text.Trim();
                var res = Bonitet.CRM.CRM_ServiceHelper.GetCRM_Account(EMBS, 2014);

                resultBox.Text = res;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(resultBox.Text);
        }
    }
}
