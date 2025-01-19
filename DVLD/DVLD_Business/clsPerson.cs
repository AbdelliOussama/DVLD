using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using System.Data;
using System.Data.SqlTypes;
using System.Security.Cryptography.X509Certificates;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DVLD_Business
{
    public class clsPerson
    {
        public enum enMode { AddNew = 1,Update = 2}
        public enMode Mode = enMode.AddNew;
        public int PersonID {  get; set; }  
        public string NationalNo {  get; set; }
        public string FirstName { get; set; }
        public string SecondName {  get; set; }
        public string ThirdName {  get; set; }
        public string LastName {  get; set; }
        public string FullName
        {
            get { return FirstName +" "+SecondName+" "+ThirdName+" "+LastName; }
        }
        public DateTime DateOfBirth { get; set; }
        public byte Gendor {  get; set; }
        public string Address {  get; set; }
        public string Phone {  get; set; }
        public string Email {  get; set; }
        public int NationalityCountryID {  get; set; }
        public clsCountry CountryInfo { get; set; }
        private string _ImagePath;
        public string ImagePath
        { 
            get { return _ImagePath; }
            set { _ImagePath = value; }
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNo = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Gendor = 0;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";
            Mode = enMode.AddNew;
            
        }
        public clsPerson(int PersonID,string NationalNo,string FirstName,string SecondName,string ThirdName,
            string LastName,DateTime DateOfBirth,byte Gendor ,string Address,string Phone,string Email,int NationalityCountryID,string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNo = NationalNo;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID =NationalityCountryID;
            this.CountryInfo  = clsCountry.Find(NationalityCountryID);
            this.ImagePath = ImagePath;
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName
                , this.LastName, this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
            return (this.PersonID != -1);
        }
        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID,this.NationalNo,this.FirstName,
                this.SecondName,this.ThirdName,this.LastName,this.DateOfBirth,this.Gendor,this.Address,this.Phone,this.Email,this.NationalityCountryID,this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string NationalNo = "", FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            byte Gendor = 0;
            bool isFound = clsPersonData.GetPersonInfoByPersonID(PersonID,ref NationalNo,ref FirstName,ref SecondName,ref ThirdName,ref LastName,ref DateOfBirth,ref Gendor,ref Address,ref Phone,ref  Email,ref NationalityCountryID,ref ImagePath);
            if (isFound)
            {
                return new clsPerson(PersonID,NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath);
            }
            else
                return null;
        }
        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            byte Gendor = 0;
            bool isFound = clsPersonData.GetPersonInfoByPersonNationalNo(NationalNo,ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath);
            if (isFound)
            {
                return new clsPerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }
        public static DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool ISPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);
        }
        public static bool ISPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
    }
}
