using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsApplication
    {
        public enum enMode { AddNew =0,Update  =1};
        public enMode Mode;

        public enum enApplicationType
        {
            NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,
            ReplaceDamagedDrivingLicense = 4, ReleaseDetainedDrivingLicsense = 5, NewInternationalLicense = 6, RetakeTest = 7
        };
        public enum enApplicationStatus { New = 1,Cancelled =  2,Completed = 3};
        public int ApplicationID {  get; set; }
        public int ApplicantPersonID {  get; set; }
        public clsPerson ApplicantPersonInfo { get; set; }
        public DateTime ApplicationDate {  get; set; }
        public enApplicationType ApplicationTypeID { get; set; }
        public clsApplicationType ApplicationTypeInfo {  get; set; }
        public enApplicationStatus ApplicationStatus { get; set; }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees {  get; set; }
        public int CreatedByUserID {  get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        public clsApplication()
        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = enApplicationType.NewDrivingLicense;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;

        }
        public clsApplication(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,enApplicationType ApplicationTypeID,enApplicationStatus ApplicationStatus,DateTime LastStatusDate,float PaidFees,int CreatedByUserID)
        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID= ApplicantPersonID;
            this.ApplicantPersonInfo = clsPerson.Find(ApplicantPersonID);
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find((int)ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindUserByUserID(CreatedByUserID);
            Mode = enMode.Update;
        }
        private bool _AddNewApplication()
        {
            this.ApplicationID = clsApplicationData.AddNewApplication(this.ApplicantPersonID,this.ApplicationDate,(int)this.ApplicationTypeID,(byte)this.ApplicationStatus,this.LastStatusDate,this.PaidFees,this.CreatedByUserID);
            return this.ApplicationID != -1;
        }
        private bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate, (int)this.ApplicationTypeID,(byte) this.ApplicationStatus, this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
        }
        public static clsApplication Find(int ApplicationID)
        {
            int ApplicantPersonID = -1, CreatedByUserID = -1, ApplicationTypeID = -1;
            byte ApplicationStatus = 0;
            DateTime ApplicationDate = DateTime.Now,LastStatusDate = DateTime.Now;
            float PaidFees = 0;
            bool IsFound = clsApplicationData.GetApplicationInfoByApplicationID(ApplicationID, ref ApplicantPersonID, ref ApplicationDate, ref ApplicationTypeID, ref ApplicationStatus, ref LastStatusDate, ref PaidFees, ref CreatedByUserID);
            if (IsFound)
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate,(clsApplication.enApplicationType) ApplicationTypeID,(clsApplication.enApplicationStatus) ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            else return null;
        }

        public static DataTable GetAllApplications()
        {
            return clsApplicationData.GetAllApplications();
        }
        public bool DeleteApplication()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID);
        }
        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }
        public static bool UpdateStatus(int ApplicationID,byte Status)
        {
            return clsApplicationData.UpdateStatus(ApplicationID, Status);
        }
        public bool CancelApplication()
        {
            return clsApplicationData.UpdateStatus(ApplicationID, 2);
        }
        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(ApplicationID,3);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateApplication();
            }
            return false;
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID,int ApplicationTypeID)
        {
            return clsApplication.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }
        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplication.DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID);
        }
        public static int GetActiveApplicationID(int PersonID,enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID,(int)ApplicationTypeID);
        }
        public  int GetActiveApplicationID( enApplicationType ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(this.ApplicantPersonID, (int)ApplicationTypeID);
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID,enApplicationType ApplicationType,int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicensseClass(PersonID,(int)ApplicationType, LicenseClassID);
        }
    }
   
}
