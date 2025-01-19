using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public  class clsTestData
    {
        public static bool GetTestInfoByTestID(int TestID,ref int TestAppointmentID,ref bool TestResult,ref string Notes,ref int CreatedByUserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Tests WHERE TestID = @TestID ";
                    using(SqlCommand command =new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@TestID", TestID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"] == DBNull.Value)
                                {
                                    Notes = "";
                                }
                                else
                                    Notes = (string)reader["Notes"];
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
                IsFound = false;
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool GetLastTestInfoByPersonAndTestTypeAndLicenseClass(int PersonID,int LicenseClassID,int TestTypeID,ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT  top 1 Tests.TestID, 
                                Tests.TestAppointmentID, Tests.TestResult, 
			                    Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
                        FROM            LocalDrivingLicenseApplications INNER JOIN
                                         Tests INNER JOIN
                                         TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                         Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                        WHERE        (Applications.ApplicantPersonID = @PersonID) 
                        AND (LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID)
                        AND ( TestAppointments.TestTypeID=@TestTypeID)
                        ORDER BY Tests.TestAppointmentID DESC";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@PersonID",PersonID);
                        command.Parameters.AddWithValue("LicenseClassID", LicenseClassID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                TestID = (int)reader["TestID"];
                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"]==DBNull.Value)
                                {
                                    Notes = "";
                                }
                                else
                                    Notes = (string)reader["Notes"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
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

        public static int AddNewTest(int TestAppointmentID,bool TestResult,string Notes,int CreatedByUserID)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Tests 
                                    (TestAppointmentID,TestResult,Notes,CreatedByUserID)
                                    VALUES(@TestAppointmentID,@TestResult,@Notes,@CreatedByUserID);
                                    UPDATE TestAppointments  
                                    SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID ;

                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command=new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);
                        if(Notes!="")
                        {
                            command.Parameters.AddWithValue("@Notes", Notes);
                        }
                        else
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            TestID = InsertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return TestID;
        }
        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Tests
                                    SET
                                    TestAppointmentID = @TestAppointmentID,
                                    TestResult = @TestResult,
                                    Notes = @Notes,
                                    CreatedByUserID = @CreatedByUesrID
                                    WHERE TestID = @TestID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestID", TestID);
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);
                        if (Notes != "")
                        {
                            command.Parameters.AddWithValue("@Notes", Notes);
                        }
                        else
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

        public static bool DeleteTest(int TestID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Tests WHEER TestID = @TestID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestID", TestID);
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

        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Tests ORDER BY TestID ";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
           byte PassedTestCount = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT PassedTestCount = COUNT(TestID) FROM Tests INNER JOIN 
                                    TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
                                    WHERE TestAppointments.LocalDrivingLicenseApplicationID  = @LocalDrivingLicenseApplicationID AND TestResult =1";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        object result = command.ExecuteScalar();
                        if(result != null && byte.TryParse(result.ToString(),out byte PassedTests))
                        {
                            PassedTestCount = PassedTests;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return PassedTestCount;
        }
    }
}
