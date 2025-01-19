using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Diagnostics.SymbolStore;

namespace DVLD_Business
{
    public  class clsDetainedLicense
    {
        public enum enMode { AddNew =0,Update = 1};
        public enMode Mode;
        public int DetainID {  get; set; }
        public int LicenseID {  get; set; }
        public clsLicense LicenseInfo { get; set; }
        public DateTime DetainDate { get; set; }
        public float FineFees {  get; set; }
        public int CreatedByuserID {  get; set; }
        public clsUser CreatedByuserInfo { get; set; }
        public bool IsReleased {  get; set; }
        public DateTime ReleaseDate {  get; set; }
        public int ReleasedByUserID {  get; set; }
        public clsUser ReleasedByUserInfo { get; set; }

        public int ReleaseApplicationID {  get; set; }

        public clsDetainedLicense()
        {
            this.DetainID = -1;
            this.LicenseID = -1;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByuserID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.Now;
            this.ReleasedByUserID = -1;
            this.ReleaseApplicationID = -1;
            Mode = enMode.AddNew;
        }
        public clsDetainedLicense( int detainID, int licenseID,DateTime detainDate, float fineFees, int createdByuserID, bool isReleased, DateTime releaseDate, int releasedByUserID, int releaseApplicationID)
        {
            this.DetainID = detainID;
            this.LicenseID = licenseID;
            this.LicenseInfo = clsLicense.Find(LicenseID);
            this.DetainDate = detainDate;
            this.FineFees = fineFees;
            this.CreatedByuserID = createdByuserID;
            this.CreatedByuserInfo =clsUser.FindUserByUserID(CreatedByuserID);
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleasedByUserInfo = clsUser.FindUserByUserID(ReleasedByUserID);
            ReleaseApplicationID = releaseApplicationID;
            Mode = enMode.Update;
        }
        private bool _AddNewDetainedLiccense()
        {
            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense(this.LicenseID, this.DetainDate, this.FineFees, this.CreatedByuserID, this.IsReleased);
            return this.DetainID != -1;
        }
        private bool _UpdateDetainedLicense()
        {
            return clsDetainedLicenseData.UpdateDetainedLicenses(this.DetainID,this.LicenseID,this.DetainDate,this.FineFees,this.CreatedByuserID,this.IsReleased);
        }

        public  static clsDetainedLicense FindByDetaindID(int DetainID)
        {
            int LicenseID = -1,CreatedByuserID = -1,ReleasedByUserID =-1,ReleaseApplicationID = -1;
            float FineFees = 0;
            DateTime DetainDate = DateTime.Now,ReleaseDate = DateTime.Now;
            bool IsReleased = false;

            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseByID(DetainID,ref LicenseID,ref DetainDate,ref FineFees,
                ref CreatedByuserID,ref IsReleased,ref ReleaseDate,ref ReleasedByUserID,ref ReleaseApplicationID);
            if(IsFound)
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByuserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;

        }
        public static clsDetainedLicense FindByLicenseID(int LicenseID)
        {
            int DetainID = -1, CreatedByuserID = -1, ReleasedByUserID = -1, ReleaseApplicationID = -1;
            float FineFees = 0;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.Now;
            bool IsReleased = false;

            bool IsFound = clsDetainedLicenseData.GetDetainedLicenseByLicenseID(LicenseID,ref DetainID, ref DetainDate, ref FineFees,
                ref CreatedByuserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleaseApplicationID);
            if (IsFound)
            {
                return new clsDetainedLicense(DetainID, LicenseID, DetainDate, FineFees, CreatedByuserID, IsReleased, ReleaseDate,
                    ReleasedByUserID, ReleaseApplicationID);
            }
            else
                return null;
        }
        public static DataTable GetAllDetainedLicense()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID,int ReleasedByApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleasedByApplicationID);
        }
        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID);
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewDetainedLiccense())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateDetainedLicense();
            }
            return false;
        }



    }
}
