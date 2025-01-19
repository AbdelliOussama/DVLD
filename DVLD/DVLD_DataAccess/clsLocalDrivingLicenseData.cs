using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.SymbolStore;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace DVLD_DataAccess
{
    public  class clsLocalDrivingLicenseData
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID,ref int ApplicationID,ref int LicenseClassID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Select * from LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                LicenseClassID = (int)reader["LicenseClassID"];
                            }
                            else
                                IsFound = false;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                IsFound = false;
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID( int ApplicationID, ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Select * from LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID",ApplicationID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                                LicenseClassID = (int)reader["LicenseClassID"];
                            }
                            else
                                IsFound = false;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                IsFound = false;
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT * FROM LocalDrivingLicenseApplications_View 
                                    ORDER BY LocalDrivingLicenseApplicationID";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {              
                Console.WriteLine("Error : " + ex.Message);
            }
            return dt;
        }

        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID,int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO LocalDrivingLicenseApplications(ApplicationID,LicenseClassID)
                                    VALUES (@ApplicationID,@LicenseClassID);
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command=new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            LocalDrivingLicenseApplicationID = InsertedID;
                        }
                            
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return LocalDrivingLicenseApplicationID;
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID,int ApplicationID,int LicenseClassID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE LocalDrivingLicenseApplications 
                                    SET
                                    ApplicationID = @ApplicationID ,
                                    LicenseClassID = @LicenseClassID
                                    WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                    using(SqlCommand command=new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @" DELETE FROM LocalDrivingLicenseApplications
                                        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
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

        public static bool DoesAttendTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT TOP 1 Found = 1
                                    FROM LocalDrivingLicenseApplications INNER JOIN 
                                        TestAppointments ON  LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID  INNER JOIN
                                        Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                                    WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                    AND (TestAppointments.TestTypeID = @TestTypeID)
                                    ORDER BY TestAppointments.TestAppointmentID DESC";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            IsFound = true;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            byte TotalTrialsPerTest = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT  TotalTrialsPerTest = COUNT(TestID)
                                        FROM LocalDrivingLicenseApplications INNER JOIN
                                            TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID  = TestAppointments.LocalDrivingLicenseApplicationID  INNER JOIN 
                                            Tests ON TestAppointments.TestAppointmentID  =Tests.TestAppointmentID  
                                        WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID ) AND (TestAppointments.TestTypeID = @TestTypeID);";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        object result = command.ExecuteScalar();
                        if (result != null && byte.TryParse(result.ToString(),out byte Trials))
                        {
                            TotalTrialsPerTest = Trials;
                        }
                    }

                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return TotalTrialsPerTest;
        }

        public static bool IsThereAnctiveScheduleTest(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connectionn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connectionn.Open();
                    string query = @"SELECT TOP 1  Found = 1 FROM LocalDrivingLicenseApplications INNER JOIN
                                    TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID  = TestAppointments.LocalDrivingLicenseApplicationID 
                                    WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                    AND (TestAppointments.TestTypeID = @TestTypeID)
                                    AND IsLocked = 0
                                    ORDER BY TestAppointments.TestAppointmentID DESC";
                    using(SqlCommand command = new SqlCommand(query,connectionn))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        object result = command.ExecuteScalar();
                        if (result != null)
                            IsFound = true;
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connectionn = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connectionn.Open();
                    string query = @"SELECT TOP 1 TestResult
                                    FROM LocalDrivingLicenseApplications INNER JOIN
                                        TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID  = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN 
                                        Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID 
                                    WHERE (LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                    AND (TestAppointments.TestTypeID = @TestTypeID)                
                                    ORDER BY TestAppointments.TestAppointmentID DESC";
                    using (SqlCommand command = new SqlCommand(query, connectionn))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        object result = command.ExecuteScalar();
                        if (result != null)
                            IsFound = true;
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }
    }
}
