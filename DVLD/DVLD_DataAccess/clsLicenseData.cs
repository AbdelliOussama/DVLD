using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.Design;
using Microsoft.Win32;

namespace DVLD_DataAccess
{
    public  class clsLicenseData
    {
        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID,ref int LicenseClass,ref DateTime IssueDate,ref DateTime ExpirationDate,
                ref string Notes,ref float PaidFees,ref bool IsActive,ref byte IssueReason,ref int CreatedByUserID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                if (reader["Notes"] != DBNull.Value)
                                    Notes = (string)reader["Notes"];
                                else
                                    Notes = "";
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                            else
                                IsFound = false;                                                  
                        }
                    }

                }
            }
            catch(SqlException ex)
            {
                IsFound  =false;
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static int AddNewLicense(  int ApplicationID,  int DriverID,  int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                string Notes, float PaidFees,bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Licenses 
                                    (ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,IssueReason,CreatedByUserID)
                                    VALUES (@ApplicationID,@DriverID,@LicenseClassID,@IssueDate,@ExpirationDate,@Notes,@PaidFees,@IsActive,@IssueReason,@CreatedByUserID);
                                    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClass);
                        command.Parameters.AddWithValue("@Issuedate",IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@Notes", Notes);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
                        command.Parameters.AddWithValue("@CreatedByuserID", CreatedByUserID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            LicenseID = InsertedID;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return LicenseID;
        }

        public static bool UpdateLicense(int LicenseID,int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
               string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Licenses  
                                    SET 
                                    ApplicationID = @ApplicationID,
                                    DriverID = @DriverID,
                                    LicenseClass = @LicenseClass,
                                    IssueDate = @IssueDate,
                                    ExpirationDate = @ExpirationDate,
                                    Notes = @Notes,
                                    PaidFees = @PaidFees,
                                    IsActive = @IsActive,
                                    IssueReason = @IssueReason,
                                    CreatedByUserID = @CreatedByuserID
                                    WHERE LicenseID = @LicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                        command.Parameters.AddWithValue("@Issuedate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@Notes", Notes);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
                        command.Parameters.AddWithValue("@CreatedByuserID", CreatedByUserID);
                       
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return RowsAffected > 0;
        }

        public static bool DeleteLicense(int LicenseID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Licenses WHERE LicenseID = @LicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);                   
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return RowsAffected > 0;
        }
        public static DataTable GetAllLicenses()
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Licenses ORDER BY LicenseID";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader  = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }               
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return table;
        }

        public static DataTable GetDriverLicenses(int DriverID)
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT     
                           Licenses.LicenseID,
                           ApplicationID,
		                   LicenseClasses.ClassName, Licenses.IssueDate, 
		                   Licenses.ExpirationDate, Licenses.IsActive
                           FROM Licenses INNER JOIN
                                LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                            where DriverID=@DriverID
                            Order By IsActive Desc, ExpirationDate Desc"; ;
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return table;
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT        Licenses.LicenseID
                            FROM Licenses INNER JOIN
                                                     Drivers ON Licenses.DriverID = Drivers.DriverID
                            WHERE  
                             
                             Licenses.LicenseClass = @LicenseClass 
                              AND Drivers.PersonID = @PersonID
                              And IsActive=1;";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("LicenseClass", LicenseClassID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int licenseID))
                             LicenseID = licenseID;
                            
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return LicenseID;
        }
        public static bool DesactivateLicense(int LicenseID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Licesnes 
                                   SET IsActive = 0 WHERE LicenseID = @LicenseID";
                    using(SqlCommand command=new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);

                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
                return false;
            }
            return RowsAffected > 0;
        }

    }
}
