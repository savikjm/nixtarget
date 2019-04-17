using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonitet.DAL
{
    public partial class PrepayPack
    {
        private string packTypeName;

        public string PackTypeName
        {
            get
            {
                if (packTypeName == null)
                {
                    switch (this.PackType)
                    {
                        case 1:
                            packTypeName = "Бонитетен извештај";
                            break;
                        case 2:
                            packTypeName = "Краток извештај";
                            break;
                        case 3:
                            packTypeName = "Блокада";
                            break;
                    }
                }

                return packTypeName;
            }
            set 
            {
                packTypeName = value;
            }
        }
    }
}