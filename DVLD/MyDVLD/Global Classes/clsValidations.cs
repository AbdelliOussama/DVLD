using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyDVLD.Global_Classes
{
    public  class clsValidations
    {
        /// <summary>
        /// This Fuction Is Used To Validate Email Format
        /// </summary>
        /// <param name="Email"></param>
        /// <returns>It Returns True If The Validation is Correct else Returns False</returns>
        public static bool ValidateEmail(string Email)
        {
            var pattern = @"^[a-zA-Z0-9.!#$%&'*+-/=?^_`{|}~]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$";
            var Regex = new Regex(pattern);
            return Regex.IsMatch(Email);

        }
    }
}
