using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestAppointment
    {
        public enum enMode { Addnew = 0,Update = 1}
        public enMode Mode  = enMode.Addnew;

        public int TestAppointmentID {  get; set; }
        public clsTestType.enTestType TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID {  get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees {  get; set; }
        public int CreatedByUserID {  get; set; }
        public bool IsLocked {  get; set; }
        public int RetakeTestApplicationID {  get; set; }
        public clsApplication RetakeTestApplicationInfo{get;set;}
        public int TestID
        {
            get { return _GetTestID(); }
        }

        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.IsLocked = false;
            this.RetakeTestApplicationID = -1;
            Mode = enMode.Addnew;
        }
        public clsTestAppointment(int TestAppointmentID,clsTestType.enTestType TestTypeID,int LocalDrivingLicenseApplicationID,DateTime AppointmentDate,
            float PaidFees,int CreatedByUserID,bool IsLocked,int RetakeTestApplicationID)
        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees= PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID= RetakeTestApplicationID;
            this.RetakeTestApplicationInfo = clsApplication.Find(RetakeTestApplicationID);
            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
            return this.TestAppointmentID != -1;
        }
        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, RetakeTestApplicationID = -1,CreatedByUserID =-1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            bool IsLocked = false,IsFound;
            IsFound = clsTestAppointmentData.GetTestAppointmentInfo(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID);
            if (IsFound)
            {
                return new clsTestAppointment(TestAppointmentID,(clsTestType.enTestType)TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID);
            }
            else
                return null;
        }
        public static clsTestAppointment FindLastTestAppointment(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            int TestAppointmentID = -1,CreatedByUserID = -1,RetakeTestApplicationID = -1;
            bool IsLocked = false,IsFound = false;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            IsFound = clsTestAppointmentData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID, ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID);
            if (IsFound)
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }
            else
                return null;
        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();
        }
        public static DataTable GetApplicationTestAppointmentsBytestType(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsByTestType(LocalDrivingLicenseApplicationID,(int) TestTypeID);
        }
        public  DataTable GetApplicationTestAppointmentsBytestType( int TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsByTestType(this.LocalDrivingLicenseApplicationID, TestTypeID);
        }
        public static bool DeleteTestAppointment(int  TestAppointmentID)
        {
            return clsTestAppointmentData.DeleteTestAppointment(TestAppointmentID);
        }
        private int  _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(this.TestAppointmentID);
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Addnew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTestAppointment();
            }
            return false;
        }
    }
}
