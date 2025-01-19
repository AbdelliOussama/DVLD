using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace DVLD_DataAccess
{
    public  class clsDetainedLicenseData
    {
        public static bool GetDetainedLicenseByID(int DetainID,ref int LicenseID,ref DateTime DetainDate,ref float FineFees,
          ref int CreatedByUserID,ref bool IsReleased,  ref DateTime ReleaseDate,ref int ReleasedByUserID,ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                LicenseID = (int)reader["LicenseID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = (float)reader["FineFees"];
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                IsReleased = (bool)reader["IsReleased"];
                                if (reader["ReleaseDate"]!= DBNull.Value)
                                {
                                    ReleaseDate = (DateTime)reader["Releasedate"];
                                }
                                else
                                    ReleaseDate = DateTime.MaxValue ;
                                if (reader["ReleasedByuqserID"] != DBNull.Value)
                                {
                                    ReleasedByUserID = (int)reader["ReleasedByuqserID"];
                                }
                                else
                                    ReleasedByUserID = -1;
                                if(reader["ReleaseApplicationID"] != DBNull.Value)
                                {
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                                }
                                else
                                    ReleaseApplicationID= -1;

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
                Console.WriteLine("An Error Occured "+ex.Message );
            }
            return IsFound;
        }

        public static bool GetDetainedLicenseByLicenseID( int LicenseID,ref int DetainID, ref DateTime DetainDate, ref float FineFees,
         ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate, ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                DetainID = (int)reader["DetainID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByUserID = (int)reader["CreatedByUserID"];
                                IsReleased = (bool)reader["IsReleased"];
                                if (reader["ReleaseDate"] != DBNull.Value)
                                {
                                    ReleaseDate = (DateTime)reader["Releasedate"];
                                }
                                else
                                    ReleaseDate = DateTime.MaxValue;
                                if (reader["ReleasedByUserID"] != DBNull.Value)
                                {
                                    ReleasedByUserID = (int)reader["ReleasedByUserID"];
                                }
                                else
                                    ReleasedByUserID = -1;
                                if (reader["ReleaseApplicationID"] != DBNull.Value)
                                {
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];
                                }
                                else
                                    ReleaseApplicationID = -1;

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
                Console.WriteLine("An Error Occured " + ex.Message);
            }
            return IsFound;
        }

        public static  DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM DetainedLicenses_View ORDER BY DetainID";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                                dt.Load(reader);
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("An Error Occured "+ex.Message );
            }
            return dt;
        }

        public static int AddNewDetainedLicense( int LicenseID,  DateTime DetainDate, float FineFees,
           int CreatedByUserID, bool IsReleased)
        {
            int DetainID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO DetainedLicenses
                                    (LicenseID ,DetainDate,FineFees,CreatedByUserID,IsReleased)
                                    VALUES
                                    (@LicenseID,@DetainDate,@FineFees,@CreatedByUserID,0);
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees",FineFees);
                        command.Parameters.AddWithValue("@CreatedByuserID", CreatedByUserID);
                        object result = command.ExecuteScalar();
                        if (result!=null && int.TryParse(result.ToString(),out int InsertedID))                       
                        {
                            DetainID = InsertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An Error Occured " +ex.Message);
            }
            return DetainID;
        }

        public static bool UpdateDetainedLicenses(int DetainID,int LicenseID, DateTime DetainDate, float FineFees,
           int CreatedByUserID, bool IsReleased)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE DetainedLicenses 
                                    SET
                                        LicenseID = @LicenseID,
                                        DetainDate = @DetainDate,
                                        FineFees = @FineFees,
                                        CreatedByUserID = @CreatedByuserID
                                    WHERE DetainID = @DetainID";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID",DetainID);
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
                        command.Parameters.AddWithValue("@CreatedByuserID", CreatedByUserID);

                        RowsAffected = command.ExecuteNonQuery();
                    }
                                    
                                    
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An Error Occured " + ex.Message);
            }
            return RowsAffected > 0;    
        }

        public static bool ReleaseDetainedLicense(int DetainID,int ReleasedByUserID,int ReleaseApplicationID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE DetainedLicenses 
                                    SET
                                    IsReleased = 1,
                                    ReleaseDate = @ReleaseDate,
                                    ReleasedByUserID = @ReleaseByUserID,
                                    ReleaseApplicationID = @ReleaseApplicationID
                                    WHERE DetainID = DetainID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
                        command.Parameters.AddWithValue("@ReleaseByUserID", ReleasedByUserID);
                        command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

                        RowsAffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("An Error Occured "+ex.Message);
            }
            return RowsAffected > 0;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"SELECT IsDetained= 1 FROM DetainedLicenses 
                                    WHERE LicenseID = @LicenseID AND IsReleased = 0";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID",LicenseID);
                        object result = command.ExecuteScalar();
                        if (result != null) 
                            IsDetained = Convert.ToBoolean(result);
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("An Error Occured "+ex.Message);
            }
            return IsDetained;
        }
        
    }
}
