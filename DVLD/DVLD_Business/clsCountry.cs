using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using System.Data;

namespace DVLD_Business
{
    public  class clsCountry
    {
        public int CountryID {  get; set; }
        public string CountryName { get; set; }

        public clsCountry()
        {
            this.CountryID = -1;
            this.CountryName = "";
        }
        public clsCountry(int countryID, string countryName)
        {
            this.CountryID = countryID;
            this.CountryName = countryName;
        }

        public static clsCountry Find(int countryID)
        {
            string countryName = "";
            bool isFound = clsCountyData.GetCountryInfoByID(countryID, ref countryName);
            if (isFound)
            {
                return new clsCountry(countryID, countryName);
            }
            else
            {
                return null;
            }
        }
        public static  clsCountry Find(string CountryName)
        {
            int CountryID = -1;
            bool isFound = clsCountyData.GetCountryInfoByCountryName(CountryName,ref CountryID);
            if (isFound)
            {
                return new clsCountry(CountryID,CountryName);
            }
            else
            {
                return null;
            }
        }

        public  static DataTable GetAllCountries()
        {
            return clsCountyData.GetAllCountries();
        }
    }
}
