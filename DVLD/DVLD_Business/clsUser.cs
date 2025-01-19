using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DVLD_DataAccess;
using System.Security.Policy;
using System.Security.AccessControl;

namespace DVLD_Business
{
    public class clsUser
    {
        public enum enMode { AddNew = 0,Update =1 }
        public enMode Mode =enMode.AddNew;

        public int UserID {  get; set; }    
        public int PersonID {  get; set; }
        public clsPerson PersonInfo { get; set; }

        public string UserName {  get; set; }
        public string Password { get; set; }
        public bool IsActive {  get; set; }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = true;

            Mode = enMode.AddNew ;
        }
        public clsUser(int UserID, int PersonID,string UserName,string Password,bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update ;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.UserName,this.Password,this.IsActive);
            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID,this.PersonID,this.UserName,this.Password,this.IsActive);
        }

        public static clsUser FindUserByUserID(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByUserID(UserID,ref PersonID,ref UserName,ref Password,ref IsActive);
            if(IsFound)
            {
                return new clsUser(UserID,PersonID,UserName,Password,IsActive); ;
            }
            else
            {
                return null;
            }
        }

        public static clsUser FindUserByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByPersonID(PersonID, ref UserID, ref UserName, ref Password, ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive); ;
            }
            else
            {
                return null;
            }
        }

        public static clsUser FindUserByUserNameAndPassword(string UserName,string Password)
        {
            int UserID = -1,PersonID=-1;
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByUserNameAndPassword(UserName,Password,ref UserID,ref PersonID,ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, UserName, Password, IsActive); ;
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateUser();
            }
            return true;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }
        public static bool DoesPersonHaveUser(int PersonID)
        {
            return clsUserData.DoesPersonHaveUser(PersonID);
        }

        public bool ChangePassword(string Password)
        {
            return clsUserData.ChangePassword(this.UserID, Password);
        }

    }
}
