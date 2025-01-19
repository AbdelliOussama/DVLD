using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsApplicationTypesData
    {
        public static bool GetApplicationTypeByApplicationTypeID(int ApplicationTypeID,ref string ApplicationTypeTitle,ref float ApplicationTypeFees)
        {
            bool isFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "Select * from ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                isFound = true;
                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationTypeFees = Convert.ToSingle(reader["ApplicationFees"]);
                            }
                            else
                                isFound = false;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return isFound;
        }

        public static int AddNewApplicationType(string ApplicationTypeTitle,float ApplicationFees)
        {
            int ApplicationTypeID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"INSERT INTO ApplicationTypes 
                                    (ApplicationTypeTitle,ApplicationFees)
                                   Values(@ApplicationTypeTitle,@ApplicationFees);
                                    SELECT SCOPE_IDENTITY();";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                        command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

                        object result = command.ExecuteScalar();  
                        if(result!= null && int.TryParse(result.ToString(), out int  InsertedID))
                        {
                            ApplicationTypeID = InsertedID;
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Error : "+ex.Message);
            }
            return ApplicationTypeID;
        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string ApplicationTypeTitle, float ApplicationFees)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"UPDATE ApplicationTypes 
                                    SET
                                    ApplicationTypeTitle = @ApplicationTypeTitle,
                                    ApplicationFees  = @ApplicationFees
                                    Where (ApplicationTypeID = @ApplicationTypeID)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                        command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

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

        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM ApplicationTypes  ORDER BY ApplicationTypeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader  = command.ExecuteReader())
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
        public static bool DeleteApplicationType(int ApplicationTypeID)
        {
            int RowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM ApplicationTypes Where ApplicationTypeID = @ApplicationTypeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);                    
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

        public static bool IsApplicationTypeExist(string ApplicationTypeTitle)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT Found = 1 From ApplicationTypes Where ApplicationTypeTitle =@ApplicationTypeTitle";
                    using(SqlCommand command = new SqlCommand(query,connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            IsFound = reader.HasRows;
                        }
                    }
                }
            }
            catch(SqlException ex)
            {
                Console.WriteLine("Error ::" + ex.Message);
            }
            return IsFound;
        }

    }
}
