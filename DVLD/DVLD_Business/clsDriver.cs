using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Runtime.Remoting;
using System.Xml.Serialization;

namespace DVLD_Business
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0,Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DriverID {  get; set; }  
        public int PersonID {  get; set; }
        public clsPerson PersonInfo { get; set; }
        public int CreatedByUserID {  get; set; }
        public clsUser UserInfo { get; set; }
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;
            Mode = enMode.AddNew;
        }
        public clsDriver(int DriverID,int PersonID,int CreatedByUserID,DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.CreatedByUserID = CreatedByUserID;
            this.UserInfo = clsUser.FindUserByUserID(CreatedByUserID);
            this.CreatedDate = CreatedDate;
            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(this.PersonID,this.CreatedByUserID,this.CreatedDate);
            return this.DriverID != -1;
        }
        private bool _UpdateDriver()
        {
            return clsDriverData.UpdateDriver(this.DriverID,this.PersonID,this.CreatedByUserID,this.CreatedDate);
        }
        public static clsDriver FindDriverByDriverID(int DriverID)
        {
            int PersonID = -1, CreatedByUserID = -1;    
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriverData.GetDriverInfoByID(DriverID,ref PersonID,ref CreatedByUserID,ref CreatedDate);
            if(IsFound) 
                return new clsDriver(DriverID,PersonID,CreatedByUserID,CreatedDate);
            else
                return null;
        }
        public static clsDriver FindDriverByPersonID(int PersonID)
        {
            int DriverID = -1, CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            bool IsFound = clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate);
            if (IsFound)
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            else
                return null;
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }
        public static bool DeleteDriver(int DriverID)
        {
            return clsDriverData.DeleteDriver(DriverID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateDriver();
            }
            return false;
        }
        public static DataTable GetAllLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }
        public static DataTable GetAllInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }
    }
}
