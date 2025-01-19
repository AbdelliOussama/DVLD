using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDVLD.Global_Classes
{
    public  class clsFormat
    {
        public static string ConvertDateToShortString(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
