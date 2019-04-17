using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bonitet.DAL.Extensions;

namespace Bonitet.DAL
{
    public partial class Company
    {
        public List<CompanyValue> _CompanyValues;
        public List<CompanyValue> CompanyValues
        {
            get
            {
                return _CompanyValues;
            }
            set
            {
                _CompanyValues = value;
            }
        }
        public List<CompanyValuesBak> _CVTemp;
        public List<CompanyValuesBak> CVTemp
        {
            get
            {
                return _CVTemp;
            }
            set
            {
                _CVTemp = value;
            }
        }
    }
}
