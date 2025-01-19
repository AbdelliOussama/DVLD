using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace MyDVLD.Global_Classes
{
    public class clsDataHelper
    {
        public static string ComputeHash(string Data)
        {
           using(SHA256 sha256 = SHA256.Create())
           {
                byte[] shabytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Data));
                return BitConverter.ToString(shabytes).Replace("-","").ToLower();
           }
           
        }
    }
}
