using DVLD_DataAccess;
using System;
using System.ComponentModel;
using System.Data;

namespace DVLD_Business
{
    public class clsInternationalLicense : clsApplication
    { 
        public enum enMode { AddNew =  0, Update = 1 }
        public enMode Mode;

        public clsDriver DriverInfo {  get; set; }
        public int InternationalLicenseID {  get; set; }
        public int ApplicationID {  get; set; }
        public int DriverID {  get; set; }
        public int IssedUsingLocalLicenseID {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive {  get; set; }
        public int CreatedByUserID {  get; set; }

        public clsInternationalLicense()
        {
            this.ApplicationTypeID = clsApplication.enApplicationType.NewInternationalLicense;

            this.InternationalLicenseID = -1;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.IssedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = true;
            this.CreatedByUserID = -1;

            Mode = enMode.AddNew;

        }
        public clsInternationalLicense(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,enApplicationStatus ApplicationStatus,
            DateTime LastStatusDate,float PaidFees,int CreatedByuserID,
            int internationalLicenseID, int applicationID, int driverID, int issedUsingLocalLicenseID, DateTime issueDate,
            DateTime expirationDate, bool isActive)
        { 
            base.ApplicationID=ApplicationID;
            base.ApplicantPersonID=ApplicantPersonID;
            base.ApplicationDate=ApplicationDate;
            base.ApplicationStatus=ApplicationStatus;
            base.LastStatusDate=LastStatusDate;
            base.PaidFees=PaidFees;
            base.CreatedByUserID=CreatedByuserID;


            InternationalLicenseID = internationalLicenseID;
            ApplicationID = applicationID;
            DriverID = driverID;
            IssedUsingLocalLicenseID = issedUsingLocalLicenseID;
            IssueDate = issueDate;
            ExpirationDate = expirationDate;
            IsActive = isActive;
            this.CreatedByUserID = CreatedByUserID;
            this.DriverInfo = clsDriver.FindDriverByDriverID(DriverID);
            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID,this.DriverID,
             this.IssedUsingLocalLicenseID,this.IssueDate,this.ExpirationDate,this.IsActive,this.CreatedByUserID);
            return (this.InternationalLicenseID != -1);
        }
        private bool _UpdateInternationallicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID,this.ApplicationID,this.DriverID,
                this.IssedUsingLocalLicenseID,this.IssueDate,this.ExpirationDate,IsActive,this.CreatedByUserID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, CreatedByUserID = -1, IssedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now,ExpirationDate = DateTime.Now;
            bool IsActive = false;
            if(clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID,ref ApplicationID
                ,ref DriverID,ref IssedUsingLocalLicenseID,ref IssueDate,ref ExpirationDate,ref IsActive,ref CreatedByUserID))
            {
                clsApplication application = clsApplication.Find(ApplicationID);

                return new clsInternationalLicense(application.ApplicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationStatus, application.LastStatusDate, application.PaidFees, CreatedByUserID, InternationalLicenseID
                   , ApplicationID, DriverID, IssedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive);
            }
            else
                return null;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
        public static int GetActiveInternationalLicenseByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInteranationalLicenseByDriverID(DriverID);
        }

        public bool Save()
        {
            base.Mode = (clsApplication.enMode)Mode;
            if(!base.Save())
                return false;

            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewInternationalLicense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateInternationallicense();
            }
            return false;
        }
    }
}
