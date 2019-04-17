using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.DAL
{
    public partial class ReportRequest
    {
        private string username;

        public string Username
        {
            get
            {
                if (username == null)
                {
                    username = this.User.Username;
                }

                return username;
            }
            set { username = value; }
        }

        public string CompanyName { get; set; }

        public string EMBS { get; set; }

        public string StatusText { get; set; }
        public bool SendMail { get; set; }
        public string ReportTypeString { get; set; }
    }
}