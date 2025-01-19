using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByApplicationID(int ApplicationID,ref int ApplicantPersonID,ref DateTime ApplicationDate,
            ref int ApplicationTypeID, ref byte ApplicationStatus,ref DateTime LastStatusDate,ref float PaidFees ,ref int CreatedByUserID)
        {
            bool IsFound=false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }
                }
            }catch (Exception ex)
            {
                IsFound = false;
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static int AddNewApplication(int ApplicantPersonID,DateTime ApplicationDate,int ApplicationTypeID,byte ApplicationStatus,
            DateTime LastStatusDate,float PaidFees,int CreatedByUserID)
        {
            int ApplicationID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Applications
                                    (ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
                                    VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,@LastStatusDate,@PaidFees,@CreatedByUserID);
                                    SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();
                        if(result!=null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            ApplicationID = InsertedID;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error :" +ex.Message);
            }
            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID,int ApplicantPersonID,DateTime ApplicationDate,int ApplicationTypeID,byte ApplicationStatus,
            DateTime LastStatusDate,float PaidFees,int CreatedByUserID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Applications
                                    SET
                                    ApplicantPersonID = @ApplicantPersonID,
                                    ApplicationDate  =@ApplicationDate,
                                    ApplicationTypeID = @ApplicationTypeID,
                                    ApplicationStatus = @ApplicationStatus,
                                    LastStatusDate = @LastStatusDate,
                                    PaidFees = @PaidFees,
                                    CreatedByUserID = @CreatedByUserID
                                    WHERE (ApplicationID = @ApplicationID)";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return RowsAffected > 0;
        }

        public static DataTable GetAllApplications()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Application ORDER BY ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return dt;
        }

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID",ApplicationID);
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return (RowsAffected > 0);

        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT Found = 1 FROM Applications WHERE ApplicationID = @ApplicationID";
                    using(SqlCommand command =new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :"+ex.Message);
            }
            return IsFound;
        }

        public static bool UpdateStatus(int ApplicationID,short NewStatus)
        {
            int RowsAffected = 0;
            try
            {
                using(SqlConnection connection  = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Applications 
                                    SET
                                    ApplicationStatus = @NewStatus
                                    WHERE ApplicationID = @ApplicationID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("ApplicationID", ApplicationID);

                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error  : "+ex.Message);
            }
            return (RowsAffected > 0);
        }
        public static bool DoesPersonHaveActiveApplication(int PersonID,int ApplicationTypeID)
        {
            return GetActiveApplicationID(PersonID, ApplicationTypeID) != -1;
        }
        public static int GetActiveApplicationID(int ApplicantPersonID,int ApplicationTypeID)
        {
            int ApplicationID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT ApplicationID FROM Applications WHERE ApplicantPersonID = @ApplicantPersonID AND ApplicationTypeID = @ApplicationTypeID AND ApplicationStatus = 1 ";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                            ApplicationTypeID = AppID;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return ApplicationID;
        }

        public static int GetActiveApplicationIDForLicensseClass(int ApplicantPersonID, int ApplicationTypeID,int LicenseClassID)
        {
            int ApplicationID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT ActiveApplicationID = Applications.ApplicationID 
                                    FROM Applications INNER JOIN LocalDrivingLicenseApplications ON
                                    Applications.ApplicationID  = LocalDrivingLicenseApplications.ApplicationID 
                                    WHERE Applications.ApplicantPersonID = @ApplicantPersonID  AND Applications.ApplicationTypeID = @ApplicationTypeID 
                                    AND LocalDrivingLicenseApplications.LicenseClassID  = @LicenseClassID
                                    AND Applications.ApplicationStatus = 1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID",ApplicationTypeID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                            ApplicationID = AppID;
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return ApplicationID;
        }

       



    }
}
