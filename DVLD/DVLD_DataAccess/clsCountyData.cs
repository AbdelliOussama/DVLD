using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Runtime.CompilerServices;

namespace DVLD_DataAccess
{
    public  class clsCountyData
    {
        public static bool GetCountryInfoByID(int CountryID ,ref string CountryName)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string qurey = "Select * from Countries Where CountryID = @CountryID ;";
                    using (SqlCommand command = new SqlCommand(qurey, connection))
                    {
                        command.Parameters.AddWithValue("@CountryID", CountryID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                CountryName = (string)reader["CountryName"];
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
                Console.WriteLine("Error : "+ex.Message);
            }
            return IsFound;
        }

        public static bool GetCountryInfoByCountryName(string CountryName ,ref int CountryID)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection  = new SqlConnection (clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "select * from Countries where CountryName = @CountryName ;";
                    using (SqlCommand command= new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CountryName", CountryName);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                IsFound = true;
                                CountryID = (int)reader["CountryID"];
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
                Console.WriteLine("Error : " + ex.Message);
            }
            return IsFound;
        }


        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"Select * from Countries
                                      Order by CountryID";
                    using(SqlCommand command= new SqlCommand(query,connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                dt.Load(reader);
                            }
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


    }
}
