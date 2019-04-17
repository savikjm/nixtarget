using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.DAL
{
    public partial class ReportRequest
    {
        public string CompanyName { get; set; }
        public string Username { get; set; }

        public string StatusText { get; set; }
        public bool SendMail { get; set; }
        public string ReportTypeString { get; set; }
    }
}