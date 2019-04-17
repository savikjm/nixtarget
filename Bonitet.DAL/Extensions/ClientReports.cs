using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.DAL.Extensions
{
    public partial class ClientReports
    {
        public int ID { get; set; }

        public string Username
        {
            get;
            set;
        }
        public string EMBS { get; set; }

        public int BonitetiTotal { get; set; }
        public bool BonitetiIsPostpaid { get; set; }

        public int FinansiskiTotal { get; set; }
        public bool FinansiskiIsPostpaid { get; set; }

        public int BlokadiTotal { get; set; }
        public bool BlokadiIsPostpaid { get; set; }

    }
}
