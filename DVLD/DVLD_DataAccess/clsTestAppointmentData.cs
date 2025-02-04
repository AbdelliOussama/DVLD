﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static bool GetTestAppointmentInfo(int TestAppointmentID,ref int TestTypeID,ref int LocalDrivingLicenseApplicationID,
                ref DateTime AppointmentDate,ref float PaidFees,ref int  CreatedByUserID,ref bool IsLocked,ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                TestTypeID = (int)reader["TestTypeID"];
                                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                PaidFees =Convert.ToSingle( reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                IsLocked = (bool)reader["IsLocked"];
                                if(reader["RetakeTestApplicationID"] !=DBNull.Value)
                                {
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                                }
                                else
                                {
                                    RetakeTestApplicationID = -1;
                                }
                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error :"+ex.Message);
            }
            return IsFound;
        }

        public static bool GetLastTestAppointment(int LocalDrivingLicenseApplicationID,int TestTypeID,ref int TestAppointmentID,
            ref DateTime AppointmentDate,ref float PaidFees,ref int CreatedByUserID,ref bool IsLocked,ref int RetakeTestApplicationID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection =new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT TOP 1 * FROM TestAppointments WHERE (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                    AND (TestTypeID = @TestTypeID) ORDER BY TestAppointmentID DESC";
                    using(SqlCommand command  = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        using(SqlDataReader reader =command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                IsLocked = (bool)reader["IsLocked"];
                                if (reader["RetakeTestApplicationID"] != DBNull.Value)
                                {
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                                }
                                else
                                {
                                    RetakeTestApplicationID = -1;
                                }
                            }
                            else
                            {
                                IsFound = false;
                            }
                        }
                    }

                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return IsFound;
        }
        public static DataTable GetAllTestAppointments()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM TestAppointments_View ORDER BY TestAppointmentID ASC";
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
                Console.WriteLine("Error :" + ex.Message);
            }
            return dt;
        }
        public static DataTable GetApplicationTestAppointmentsByTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT TestAppointmentID,AppointmentDate,PaidFees,IsLocked FROM TestAppointments
                                    WHERE (LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID)
                                    AND ( TestTypeID = @TestTypeID)
                                    ORDER BY TestAppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID",TestTypeID);
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
                Console.WriteLine("Error :" + ex.Message);
            }
            return dt;

        }
        public static int AddNewTestAppointment(int TestTypeID,int LocalDrivingLicenseApplicationID,DateTime AppointmentDate,
            float PaidFees,int CreatedByUserID,bool IsLocked,int RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO TestAppointments (TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,
                                    IsLocked,RetakeTestApplicationID)
                                    Values(@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,@IsLocked,@RetakeTestApplicationID);
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate",AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@IsLocked", IsLocked);
                        if(RetakeTestApplicationID ==-1)
                        {
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                        }
                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(),out int InsertedID))
                        {
                            TestAppointmentID = InsertedID;
                        }                       
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return TestAppointmentID;
        }
        public static bool UpdateTestAppointment(int TestAppointmentID,int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
            float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            int RowsAffected = 0;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE TestAppointments
                                      SET 
                                      TestTypeID = @TestTypeID,
                                      LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                                      AppointmentDate = @AppointmentDate,
                                      PaidFees = @PaidFees,
                                      CreatedByUserID = @CreatedByUserID,
                                      IsLocked = @IsLocked,
                                      RetakeTestApplicationID = @RetakeTestApplicationID
                                      WHERE TestAppointmentID = @TestAppointmentID ";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate",AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@IsLocked", IsLocked);
                        if (RetakeTestApplicationID == -1)
                        {
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);
                        }
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return RowsAffected > 0;
        }
        public static bool DeleteTestAppointment(int TestAppointmentID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);   
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return RowsAffected > 0;
        }

        public static int GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(),out int  SelectedTestID))
                        {
                            TestID = SelectedTestID;
                        }
                         
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error :" + ex.Message);
            }
            return TestID;
        }
    }
}
