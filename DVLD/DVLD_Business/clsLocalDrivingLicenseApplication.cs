using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;

namespace DVLD_Business
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew =  0, Update = 1 }
        public enMode Mode;
        public int LocalDrivingLicenseApplicationID {  get; set; }
        public int LicenseClassID {  get; set; }
        public clsLicenseClass LicenseClassInfo { get; set; }
        public string PersonFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }

        }

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            Mode = enMode.AddNew;
        }
        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int LicenseClassID,int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,enApplicationType ApplicationTypeID,enApplicationStatus ApplicationStatus,DateTime LastStatusDate,float PaidFees,int CreatedByUserID)
        {
            this.LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            this.LicenseClassID= LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            this.ApplicationID= ApplicationID;
            this.ApplicantPersonID= ApplicantPersonID;
            this.ApplicationDate= ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate= LastStatusDate;
            this.PaidFees= PaidFees;
            this.CreatedByUserID= CreatedByUserID;

            Mode = enMode.Update;
        }

        private  bool _AddNewLoacalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseData.AddNewLocalDrivingLicenseApplication(this.ApplicationID, this.LicenseClassID);
            return this.LocalDrivingLicenseApplicationID != -1;
        }
        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseData.UpdateLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID,this.ApplicationID,this.LicenseClassID); ;
        }
        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1,  LicenseClassID = -1;
           

            bool IsFound = clsLocalDrivingLicenseData.GetLocalDrivingLicenseApplicationInfoByID(LocalDrivingLicenseApplicationID,ref ApplicationID,ref LicenseClassID);
            if (IsFound)
            {
                clsApplication application = clsApplication.Find(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, LicenseClassID, application.ApplicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, application.ApplicationStatus, application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
            }
            else
                return null;
        }
        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;


            bool IsFound = clsLocalDrivingLicenseData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID,ref LocalDrivingLicenseApplicationID, ref LicenseClassID);
            if (IsFound)
            {
                clsApplication application = clsApplication.Find(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, LicenseClassID, application.ApplicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, application.ApplicationStatus, application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
            }
            else
                return null;
        }       

        public bool Save()
        {
            base.Mode = (clsApplication.enMode) Mode;
            if(!base.Save())
                return false;
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLoacalDrivingLicenseApplication())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateLocalDrivingLicenseApplication();
            }
            return false;
        }
       
        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseData.GetAllLocalDrivingLicenseApplications();  
        }
        public  bool DeleteLocalDrivingLicenseApplication()
        {
            bool IsLocalDrivingLicenseApplicationDeleted  =false;
            bool IsBaseApplicationDeleted = false;
            IsLocalDrivingLicenseApplicationDeleted  =clsLocalDrivingLicenseData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
            if(!IsLocalDrivingLicenseApplicationDeleted )
                return false;
            IsBaseApplicationDeleted = base.DeleteApplication();
            return IsBaseApplicationDeleted;
        }
        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.IsThereAnctiveScheduleTest(LocalDrivingLicenseApplicationID,(int)TestTypeID) ;
        }
        public  bool IsThereAnActiveScheduledTest( clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.IsThereAnctiveScheduleTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.DoesPassTestType(LocalDrivingLicenseApplicationID,(int) TestTypeID);
        }
        public  bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public  clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.GetLastTestPerTestPertestType(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        public  static byte GetPassedTests(int LocalDrivingLicenseApplication)
        {
            return clsTest.GetPassedTest(LocalDrivingLicenseApplication);
        }
        public  byte GetPassedTests()
        {
            return clsTest.GetPassedTest(this.LocalDrivingLicenseApplicationID);
        }
        public static bool DoesPassAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.DoesPassedAllTests(LocalDrivingLicenseApplicationID);
        }
        public  bool DoesPassAllTests()
        {
            return clsTest.DoesPassedAllTests(this.LocalDrivingLicenseApplicationID);
        }

        public int  IssueLicenseForTheFirstTime(string Notes ,int CreatedByUserID)
        {
            int DriverID = -1;
            clsDriver Driver = clsDriver.FindDriverByDriverID(this.ApplicantPersonID);
            if (Driver == null)
            {
                Driver = new clsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                    DriverID = -1;
            }
            else
                DriverID = Driver.DriverID;


            clsLicense License = new clsLicense();
            License.DriverID = DriverID;
            License.CreatedByUserID = CreatedByUserID;
            License.ApplicationID = this.ApplicationID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(clsLicenseClass.Find(this.LicenseClassID).DefaultValidityLength);
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.LicenseClass = this.LicenseClassID;

            if (License.Save())
            {
                this.SetComplete();

                return License.LicenseID;
            }
            else
                return -1;

        }
        public bool IsLicesneIssued()
        {
            return (GetActiveLicenseID()!=-1);
        }
        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(this.ApplicantPersonID,this.LicenseClassID);
        }
       
    }
}
