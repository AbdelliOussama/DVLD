using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID,ref int ApplicationID,ref  int DriverID,ref int IssuedUsingLocalLicenseID
        ,ref DateTime IssueDate,ref DateTime ExpirationDate,ref bool IsActive,ref int CreatedByUserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT * FROM InternationalLicenses WHERE InternationalLicenseID = @InternationalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                IsActive = (bool)reader["IsActive"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                            else
                                IsFound = false;
                        }
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                IsFound = false;
                Console.WriteLine("Error "+sqlEx.Message);
            }
            return IsFound;
        }

        public static int AddNewInternationalLicense(int ApplicationID,int DriverID,int IssuedUsingLocalLicenseID,DateTime IssueDate,DateTime ExpirationDate,bool IsActive,int CreatedByUserID)
        {
            int InternationalLicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                                    Update InternationalLicenses 
                                    set IsActive=0
                                    where DriverID=@DriverID;
                                    
                                    INSERT INTO InternationalLicenses 
                                    (
                                     ApplicationID,
                                     DriverID,IssuedUsingLicenseID,
                                     IssueDate,
                                     ExpirationDate,
                                     IsActive,
                                     CreatedByuserID
                                    )
                                    VALUES
                                    (
                                       @ApplicationID,
                                       @DriverID,
                                       @IssuedUsingLicenseID,
                                       @IssueDate, 
                                       @ExpirationDate,
                                       @IsActive,
                                       @CreatedByUserID
                                    );
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            InternationalLicenseID = InsertedID;
                        }

                    }


                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error : "+sqlEx.Message);
            }
            return InternationalLicenseID;
        }

        public static bool UpdateInternationalLicense(int InternationalLicenseID,int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE InternationalLicense 
                                    SET
                                    ApplicationID = @ApplicationID,
                                    DriverID =@DriverID,
                                    IssuedUsingLicenseID = @IssedUsingLicenseID,
                                    IssueDate = @IssueDate,
                                    ExpirationDate = @ExpirationDate,
                                    IsActive = @IsActive,
                                    CreatedByuserID = @CreatedByUserID
                                    WHERE InternationalLicenseID = @InternationalLicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                       RowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error : " + sqlEx.Message);
            }
            return RowsAffected > 0;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM InternationalLicenses ORDER BY InternationalLicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch(SqlException sqlEx)
            {
                Console.WriteLine("Error : "+sqlEx.Message);
            }
            return table;
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            DataTable table = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT InternationaLicenseID,ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive
                                    FROM InternationalLicenses WHERE DriverID = @DriverID 
                                    ORDER BY InterantionalLicenseID DESC";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                table.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error : " + sqlEx.Message);
            }
            return table;
        } 
        public static int GetActiveInteranationalLicenseByDriverID(int DriverID)
        {
            int InternationalLicenseID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT Top 1 InternationalLicenseID
                                    FROM InternationalLicenses 
                                    where DriverID=@DriverID and GetDate() between IssueDate and ExpirationDate 
                                    order by ExpirationDate Desc;";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(),out int LicenseID))
                        {
                            InternationalLicenseID = LicenseID;
                        }
                    }
                }
            } 
            catch (SqlException sqlEx)
            {
                Console.WriteLine("Error : "+sqlEx.Message);
            }
            return InternationalLicenseID;
        }
    }
}
