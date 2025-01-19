using DVLD_Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows.Forms;

namespace MyDVLD.Global_Classes
{
    public class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUserNameAndPassword(string UserName,string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\MyDVLD_APP";
            string ValueName1 = "UserName";
            string ValueName2 = "Password";
            string ValueData1 = UserName;
            string ValueData2 = Password;

            try
            {
                Registry.SetValue(KeyPath, ValueName1, ValueData1);
                Registry.SetValue(KeyPath, ValueName2, ValueData2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error "+ex.Message);
                return false;
            }
            return true;
        }


        public static bool GetStoredCredential(ref string UserName,ref string Password)
        {
            string KeyPath = @"HKEY_CURRENT_USER\SOFTWARE\MyDVLD_APP";
            string ValueName1 = "UserName";
            string ValueName2 = "Password";

            try
            {
                UserName = Registry.GetValue(KeyPath, ValueName1, null) as string;
                Password = Registry.GetValue(KeyPath, ValueName2, null) as string;            
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: "+ex.Message);
                return false;
            }
            return UserName != null && Password != null ? true : false;
        }
    }
}
