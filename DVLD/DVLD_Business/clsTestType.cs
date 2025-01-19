using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsTestType
    {
        public enum enTestType { VisionTest = 1,WrittenTest = 2,StreetTest = 3 }
        public enum enMode { AddNew = 0, Update = 1 }
        public enMode Mode;

        public enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription {  get; set; }
        public float TestTypeFees { get; set; }

        public clsTestType()
        {
            this.TestTypeID = enTestType.VisionTest;
            this.TestTypeTitle = "";
            this.TestTypeDescription = "";
            this.TestTypeFees = 0;

            Mode = enMode.AddNew;
        }
        public clsTestType(clsTestType.enTestType TestTypeID, string testTypeTitle,string testTypeDescription, float TestTypeFees)
        {
            this.TestTypeID = TestTypeID;
            this.TestTypeTitle = testTypeTitle;
            this.TestTypeDescription = testTypeDescription;
            this.TestTypeFees = TestTypeFees;
            Mode = enMode.Update;

        }

        private bool _AddNewTestType()
        {
            this.TestTypeID = (enTestType)clsTestTypesData.AddNewTestType(this.TestTypeTitle,this.TestTypeDescription,this.TestTypeFees);
            return this.TestTypeTitle!="";
        }
        private bool _UpddatetestType()
        {
            return clsTestTypesData.UpdateTestType((int)this.TestTypeID, this.TestTypeTitle,this.TestTypeDescription, this.TestTypeFees);
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string TestTypeTitle = "",TestTypeDescription ="";
            float TestTypeFees = 0;
            bool IsFound = clsTestTypesData.GetTestTypeByTestTypeID((int)TestTypeID, ref TestTypeTitle,ref TestTypeDescription, ref TestTypeFees);

            if (IsFound)
            {
                return new clsTestType(TestTypeID, TestTypeTitle, TestTypeDescription,TestTypeFees);
            }
            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }
        public static bool DeleteTestType(int TestTypeID)
        {
            return clsTestTypesData.DeleteTestType(TestTypeID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpddatetestType();
            }
            return true;
        }
    }
}
