using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Security.AccessControl;
using System.CodeDom;

namespace DVLD_DataAccess
{
    public  class clsDriverData
    {
        public static bool GetDriverInfoByID(int DriverID,ref int PersonID,ref int CreatedByUserID,ref DateTime CreatedDate)
        {
            bool IsFound =false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                PersonID = (int)reader["PersonID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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
                IsFound=false;
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool GetDriverInfoByPersonID( int PersonID, ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                DriverID = (int)reader["DriverID"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];
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
                IsFound = false;
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }

        public static int AddNewDriver(int PersonID,int CreatedByUserID,DateTime CreatedDate)
        {
            int DriverID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO Drivers (PersonID,CreatedByUserID,CreatedDate)
                                    VALUES (@PersonID,@CreatedByUserID,@CreatedDate);
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@personID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByuserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString() ,out int InsertedID))
                        {
                            DriverID = InsertedID;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return DriverID;
        }

        public static bool UpdateDriver(int DriverID,int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE Drivers 
                                    SET 
                                    PersonID = @PersonID,
                                    CreatedByUserID = @CreatedByUserID,
                                    CreatedDate = @CreatedDate
                                    WHERE DriverID = @DriverID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
                        command.Parameters.AddWithValue("@CreatedDate", CreatedDate);
                        RowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return RowsAffected>0;
        }


        public static bool DeleteDriver(int DriverID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Drivers WHERE DriverID = @DriverID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        RowsAffected = command.ExecuteNonQuery() ;
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return RowsAffected > 0;
        }

        public static DataTable GetAllDrivers()
        {
            DataTable table = new DataTable();
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Drivers_View ORDER BY DriverID";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
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
                Console.WriteLine("Error :"+ex.Message);
            }
            return table;
        }
    }
}
