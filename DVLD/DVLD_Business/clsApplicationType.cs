using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsApplicationType
    {
        public enum enMode { AddNew =  0, Update = 1 }
        public enMode Mode ;

        public int ApplicationTypeID {  get; set; }
        public string ApplicationTypeTitle { get; set; }
        public float ApplicationFees {  get; set; }

        public clsApplicationType()
        {
            this.ApplicationTypeID = -1;
            this.ApplicationTypeTitle = "";
            this.ApplicationFees = 0;

            Mode = enMode.AddNew;
        }
        public clsApplicationType(int ApplicationTypeID,string ApplicationTypeTitle,float ApplicationTypeFees)
        {
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeTitle = ApplicationTypeTitle;
            this.ApplicationFees= ApplicationTypeFees;
            Mode = enMode.Update;

        }

        private bool _AddNewApplicationType()
        {
            this.ApplicationTypeID =clsApplicationTypesData.AddNewApplicationType(this.ApplicationTypeTitle,this.ApplicationFees);
            return this.ApplicationTypeID != -1;
        }
        private bool _UpddateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.ApplicationTypeID,this.ApplicationTypeTitle,this.ApplicationFees);
        }

        public static clsApplicationType Find(int ApplicationTypeID)
        {
            string ApplicationTypeTitle = "";
            float ApplicationTypeFees = 0;
            bool IsFound = clsApplicationTypesData.GetApplicationTypeByApplicationTypeID(ApplicationTypeID,ref  ApplicationTypeTitle,ref ApplicationTypeFees);

            if (IsFound)
            {
                return new clsApplicationType(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
            }
            else
                return null;
        }

        public static  DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }
        public static bool DeleteApplicationType(int ApplicationTypeID)
        {
            return clsApplicationTypesData.DeleteApplicationType(ApplicationTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpddateApplicationType();
            }
            return false;
        }

        public static bool IsApplicationTypeExist(string ApplicationTypeTitle)
        {
            return clsApplicationTypesData.IsApplicationTypeExist(ApplicationTypeTitle);
        }
    }
}
