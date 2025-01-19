using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Security.Cryptography;

namespace DVLD_Business
{
    public class clsTest
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public int TestID { get; set; }
        public int TestAppointmentID { get; set; }
        public clsTestAppointment TestAppointmentInfo { get; set; }
        public bool TestResult { get; set; }
        public string Notes { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }

        public clsTest()
        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes = "";
            this.CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        public clsTest(int TestID, int TestAppointment, bool TestResult, string Notes, int CretedByUserID)
        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointment;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CretedByUserID;
            this.CreatedByUserInfo = clsUser.FindUserByPersonID(CreatedByUserID);
        }
        private bool _AddNewTest()
        {
            this.TestID = clsTestData.AddNewTest(TestAppointmentID, this.TestResult, this.Notes, this.CreatedByUserID);
            return this.TestID != -1;
        }
        private bool _UpdateTest()
        {
            return clsTestData.UpdateTest(this.TestID,this.TestAppointmentID,this.TestResult,this.Notes, this.CreatedByUserID);
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1,CreatedByUqerID = -1;
            bool TestResult = false;
            string Notes = "";

            bool IsFound = clsTestData.GetTestInfoByTestID(TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUqerID);
            if(IsFound)
                return new clsTest(TestID, TestAppointmentID,TestResult,Notes,CreatedByUqerID);
            else
                return null;
        }
        public static clsTest GetLastTestPerTestPertestType(int PersonID,int LicenseClassID,clsTestType.enTestType TestTypeID)
        {
            int TestID = -1,TestAppointmentID = -1, CreatedByUqerID = -1;
            bool TestResult = false;
            string Notes = "";

            bool IsFound = clsTestData.GetLastTestInfoByPersonAndTestTypeAndLicenseClass(PersonID,LicenseClassID,(int)TestTypeID,ref TestID, ref TestAppointmentID, ref TestResult, ref Notes, ref CreatedByUqerID);
            if (IsFound)
                return new clsTest(TestID, TestAppointmentID, TestResult, Notes, CreatedByUqerID);
            else
                return null;
        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();
        }
        public static bool DeleteTest(int TestID)
        {
            return clsTestData.DeleteTest(TestID);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTest())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateTest();
            }
            return false;
        }

        public static byte GetPassedTest(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool DoesPassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return GetPassedTest(LocalDrivingLicenseApplicationID) == 3;
        }
    }
}
