using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0,Update  = 1}
        public enMode Mode ;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public int LicenseID {  get; set; }
        public int ApplicationID {  get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo;
        public int LicenseClass {  get; set; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes {  get; set; }
        public float PaidFees {  get; set; }
        public bool IsActive {  get; set; }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get { return GetIssueReasonText(this.IssueReason); }
        }
        public int CreatedByUserID {  get; set; } 
        public clsDetainedLicense DetainInfo { get; set; }

        public bool IsDetained { get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID); } }

        public clsLicense()
        {
            this.LicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = false;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
            
        }
        public clsLicense(int LicenseID,int ApplicationID,int DriverID,int LicenseClass,DateTime IssueDate,DateTime ExpirationDate,string Notes,float PaidFees,
            bool IsActive,enIssueReason IssueReason,int CreatedByuserID)
        {
            this.LicenseID= LicenseID;
            this.ApplicationID= ApplicationID;
            this.DriverID= DriverID;
            this.DriverInfo = clsDriver.FindDriverByDriverID(this.DriverID);
            this.LicenseClass= LicenseClass;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClass);
            this.IssueDate= IssueDate;
            this.ExpirationDate=ExpirationDate;
            this.Notes = Notes;
            this.PaidFees= PaidFees;
            this.IsActive=IsActive;
            this.IssueReason= IssueReason;
            this.CreatedByUserID = CreatedByuserID;
            this.DetainInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);
            Mode = enMode.Update;
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,this.IssueDate,this.ExpirationDate,this.Notes,this.PaidFees,this.IsActive,(byte)this.IssueReason,this.CreatedByUserID);
            return this.LicenseID != -1;
        }
        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseID, this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1,CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now,ExpirationDate = DateTime.Now;
            float PaidFees = 0;
            string Notes = "";
            bool IsActive = false;
           byte IssueReason = 1;

            bool IsFound = clsLicenseData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID);
            if(IsFound)
            {
                return new clsLicense(LicenseID,ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,(enIssueReason)IssueReason,CreatedByUserID);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }
        public static bool DeleteLicense(int LicenseID)
        {
            return clsLicenseData.DeleteLicense(LicenseID);
        }
        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }

        public bool IsLicenseExistsByPersonID(int PersonID,int LicenseClassID)
        {
            return GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1;
        }

        public bool IsLicenseExpired()
        {
            return this.ExpirationDate < DateTime.Now;
        }
        public bool DesactivateCurrentLicense()
        {
            return clsLicenseData.DesactivateLicense(this.LicenseID);
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID,int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        public string GetIssueReasonText(enIssueReason IssueReason)
        {
            switch(IssueReason)
            {
                case enIssueReason.FirstTime:
                    return " First Time";
                case enIssueReason.Renew:
                    return " Renew ";
                case enIssueReason.DamagedReplacement:
                    return "Replacment For Damaged";
                case enIssueReason.LostReplacement:
                    return " Replacment For Lost";
                default:
                    return "First Time";
            }
        }

        public clsLicense RenewLicense(string Notes, int CretedByUserID)
        {
            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = DriverID;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.LicenseClass = LicenseClass;


            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;

            if (!NewLicense.Save())
            {
                return null;
            }

            DesactivateCurrentLicense();

            return NewLicense;
        }

        public clsLicense Replace(clsLicense.enIssueReason IssueReason,int CreatedByUserID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = IssueReason == enIssueReason.DamagedReplacement ? clsApplication.enApplicationType.ReplaceDamagedDrivingLicense : clsApplication.enApplicationType.ReplaceLostDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;


            if (!Application.Save())
                return null;


            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = DriverID;
            NewLicense.CreatedByUserID = CreatedByUserID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.LicenseClass = LicenseClass;


            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = 0;
            NewLicense.IsActive = true;

            if (!NewLicense.Save())
            {
                return null;
            }

            DesactivateCurrentLicense();

            return NewLicense;
        } 
        public int DetainLicense(float FineFees,int CreatedByuserID)
        {
            clsDetainedLicense DetainedLicese = new clsDetainedLicense();
            DetainedLicese.LicenseID = this.LicenseID;
            DetainedLicese.CreatedByuserID = CreatedByuserID;
            DetainedLicese.FineFees = Convert.ToSingle(FineFees);
            DetainedLicese.DetainDate = DateTime.Now;   

            if(!DetainedLicese.Save())
                return -1;

            return DetainedLicese.DetainID;
        } 
        public bool ReleaseLicense(int ReleasedByuserID,ref int ApplicationID)
        {
            clsApplication application = new clsApplication();
            application.ApplicantPersonID = this.DriverInfo.PersonID;
            application.ApplicationDate = DateTime.Now;
            application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            application.ApplicationTypeID = clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).ApplicationFees;
            application.CreatedByUserID = CreatedByUserID;
            application.LastStatusDate = DateTime.Now;
            if(!application.Save())
            {
                ApplicationID = -1;
                return false;
            }
            ApplicationID = application.ApplicationID;

            return this.DetainInfo.ReleaseDetainedLicense(ReleasedByuserID, ApplicationID);
        }
    }
}
